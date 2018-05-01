using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Resilience.Http;

namespace User.Identity.Infrastructure
{
    public class ResilienceClientFactory
    {
        private readonly ILogger<ResilienceClientFactory> _logger;
        private readonly ILogger<ResilientHttpClient> _loggerHttpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _retryCount;
        private readonly int _exceptionCountAllowedBeforeBreaking;

        public ResilienceClientFactory(ILogger<ResilienceClientFactory> logger, IHttpContextAccessor httpContextAccessor, int retryCount, int exceptionCountAllowedBeforeBreaking, ILogger<ResilientHttpClient> loggerHttpClient)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _retryCount = retryCount;
            _exceptionCountAllowedBeforeBreaking = exceptionCountAllowedBeforeBreaking;
            _loggerHttpClient = loggerHttpClient;
        }

        private IEnumerable<IAsyncPolicy> GetAsyncPolicyArray(string origin)
        {
            var arr = new IAsyncPolicy[]
            {
                Policy.Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        // number of retries
                        _retryCount,
                        // exponential backofff
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                        // on retry
                        (exception, timeSpan, retryCount, context) =>
                        {
                            var msg = $"Retry {retryCount} implemented with Polly's RetryPolicy " +
                                      $"of {context.PolicyKey} " +
                                      $"at {context.OperationKey}, " +
                                      $"due to: {exception}.";
                            _logger.LogWarning(msg);
                            _logger.LogDebug(msg);
                        }),
                Policy.Handle<HttpRequestException>()
                    .CircuitBreakerAsync( 
                        // number of exceptions before breaking circuit
                        _exceptionCountAllowedBeforeBreaking,
                        // time circuit opened before retry
                        TimeSpan.FromMinutes(1),
                        (exception, duration) =>
                        {
                            // on circuit opened
                            _logger.LogTrace("Circuit breaker opened");
                        },
                        () =>
                        {
                            // on circuit closed
                            _logger.LogTrace("Circuit breaker reset");
                        })
            };
            return arr;
        }

        public IHttpClient GetResilientHttpClient() =>
            new ResilientHttpClient(origin => GetAsyncPolicyArray(origin), _loggerHttpClient, _httpContextAccessor);
    }
}
