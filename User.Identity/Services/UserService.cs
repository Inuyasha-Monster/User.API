using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace User.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _userServiceUrl = "http://localhost:5000";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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