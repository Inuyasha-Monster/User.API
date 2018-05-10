using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReCommand.API.Options;
using Resilience.Http;

namespace ReCommand.API.Service
{
    public class ContactService : IContactService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger<UserService> _logger;
        private readonly string _contactServiceUrl;

        public ContactService(IHttpClient httpClient, ILogger<UserService> logger, IDnsQuery dnsQuery, IOptions<ServiceDisvoveryOptions> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            var result = dnsQuery.ResolveService("service.consul", options.Value.ContactServiceName);
            var addressList = result.First().AddressList;
            var address = addressList.Any() ? addressList.First().ToString() : result.First().HostName;
            var port = result.First().Port;
            _contactServiceUrl = $"http://{address}:{port}";
        }

        public async Task<List<Dtos.Contact>> GetContactListByUserIdAsync(int userId)
        {
            List<Dtos.Contact> contacts = null;
            try
            {
                var reponse = await _httpClient.GetStringAsync($"{_contactServiceUrl}/api/get/{userId}");
                if (!string.IsNullOrWhiteSpace(reponse))
                {
                    contacts = JsonConvert.DeserializeObject<List<Dtos.Contact>>(reponse);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "GetContactListByUserIdAsync请求发生错误");
            }
            return contacts;
        }
    }
}