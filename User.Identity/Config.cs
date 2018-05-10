using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace User.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("gateway_api","gateway api service"),
                new ApiResource("gateway_userapi","user service"),
                new ApiResource("gateway_contactapi","contact service"),
                new ApiResource("gateway_projectapi","project service"),
                new ApiResource("gateway_recommandapi","recommand service")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId = "android",
                    ClientName = "android",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    AllowedGrantTypes = new List<string>(){"sms_auth_code"} ,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "gateway_api",
                        "gateway_userapi",
                        "gateway_contactapi",
                        "gateway_projectapi",
                        "gateway_recommandapi",
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }
                }
            };
        }
    }
}
