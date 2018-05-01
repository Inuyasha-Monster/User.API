﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DnsClient;
using Microsoft.Extensions.Options;
using User.Identity.Options;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _userServiceUrl;

        public UserService(HttpClient httpClient, IDnsQuery dnsQuery, IOptions<ServiceDisvoveryOptions> options)
        {
            _httpClient = httpClient;
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
            var reponse = await _httpClient.PostAsync($"{_userServiceUrl}/api/users/check-or-create", new FormUrlEncodedContent(form));

            if (reponse.StatusCode == HttpStatusCode.OK)
            {
                var id = await reponse.Content.ReadAsStringAsync();
                int.TryParse(id, out int userid);
                return userid;
            }
            return await Task.FromResult(0);
        }
    }
}