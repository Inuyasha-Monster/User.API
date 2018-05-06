using System;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Contact.API.Options;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Resilience.Http;

namespace Contact.API.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly string _userServiceUrl;

        public UserService(IHttpClient httpClient, ILogger<UserService> logger, IDnsQuery dnsQuery, IOptions<ServiceDisvoveryOptions> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            var result = dnsQuery.ResolveService("service.consul", options.Value.UserServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName;
            var port = result.First().Port;
            _userServiceUrl = $"http://{address}:{port}";
        }

        public async Task<BaseUserInfo> GetBaseUserInfoAsync(int userId)
        {
            BaseUserInfo result = null;
            try
            {
                var reponse = await _httpClient.GetStringAsync($"{_userServiceUrl}/api/users/baseinfo/{userId}");
                if (!string.IsNullOrWhiteSpace(reponse))
                {
                    result = JsonConvert.DeserializeObject<BaseUserInfo>(reponse);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "调用Http服务BaseInfo失败");
            }
            return result;
        }
    }
}