using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReCommand.API.Options;
using Resilience.Http;

namespace ReCommand.API.Service
{
    public class ContactService : IContactService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly string _userServiceUrl;

        public ContactService(IHttpClient httpClient, ILogger<UserService> logger, IDnsQuery dnsQuery, IOptions<ServiceDisvoveryOptions> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            var result = dnsQuery.ResolveService("service.consul", options.Value.ContactServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName;
            var port = result.First().Port;
            _userServiceUrl = $"http://{address}:{port}";
        }

        public async Task<List<Dtos.Contact>> GetContactListByUserIdAsync(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}