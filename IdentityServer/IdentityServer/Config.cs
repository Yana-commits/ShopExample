using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new ApiResource[]
            {
                new ApiResource("alevelwebsite.com")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("mvc")
                    },
                },
                new ApiResource("fruitcatalog")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("fruitcatalog.fruitcatalogbff"),
                        new Scope("fruitcatalog.fruitcatalogitem"),
                    },
                },
                new ApiResource("basket")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("basket.basketCache.api")
                    }
                },
                new ApiResource("order")
                {
                    Scopes = new List<Scope>
                    {
                        new Scope("order.order.api"),
                        new Scope("order.orderbff.api"),
                        new Scope("order.orderitem.api")
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new[]
            {
                new Client
                {
                    ClientId = "mvc_pkce",
                    ClientName = "MVC PKCE Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    RedirectUris = { $"{configuration["MvcUrl"]}/signin-oidc"},
                    AllowedScopes = {"openid", "profile", "mvc"},
                    RequirePkce = true,
                    RequireConsent = false
                },
                new Client
                {
                    ClientId = "fruitcatalog",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret     for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                },
                new Client
                {
                    ClientId = "fruitcatalogswaggerui",
                    ClientName = "Fruit Catalog Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["FruitCatalogApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["FruitCatalogApi"]}/swagger/" },

                    AllowedScopes =
                    {
                       "mvc", "fruitcatalog.fruitcatalogbff", "fruitcatalog.fruitcatalogitem"
                    }
                },
                new Client
                {
                    ClientId = "basket",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                         "order.order.api"
                    }
                },
                new Client
                {
                    ClientId = "basketswaggerui",
                    ClientName = "Basket Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["BasketApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["BasketApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "basket.basketCache.api",
                        "order.order.api",
                        "mvc"
                    }
                },
                new Client
                {
                    ClientId = "order",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        "basket.basketCache.api"
                    }
                },
                new Client
                {
                    ClientId = "orderswaggerui",
                    ClientName = "Order Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{configuration["OrderApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{configuration["OrderApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "basket.basketCache.api",
                        "order.order.api",
                        "order.orderbff.api",
                        "order.orderitem.api"
                    }
                }
            };
        }
    }
}