using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resilience.Http;
using User.Identity.Options;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly string _userServiceUrl;

        public UserService(IHttpClient httpClient, IDnsQuery dnsQuery, IOptions<ServiceDisvoveryOptions> options, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            var result = dnsQuery.ResolveService("service.consul", options.Value.UserServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName;
            var port = result.First().Port;
            _userServiceUrl = $"http://{address}:{port}";
        }

        public async Task<int> CheckOrCreateAsync(string phone)
        {
            var form = new Dictionary<string, string>()
            {
                {"phone" , phone }
            };
            try
            {
                var reponse = await _httpClient.PostAsync($"{_userServiceUrl}/api/users/check-or-create", new FormUrlEncodedContent(form));
                if (reponse.StatusCode == HttpStatusCode.OK)
                {
                    var id = await reponse.Content.ReadAsStringAsync();
                    int.TryParse(id, out int userid);
                    _logger.LogTrace($"创建获取成功: {userid}");
                    return userid;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CheckOrCreateAsync重试失败:");
                throw;
            }

            return await Task.FromResult(0);
        }
    }
}