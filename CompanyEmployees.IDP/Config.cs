﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace CompanyEmployees.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> Ids =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResource("roles", "User role(s)", new List<string> { "role" }),
            new IdentityResource("country", "Your country", new List<string> { "country" })

        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { 
                new ApiScope("companyemployeeapi.scope", "CompanyEmployee Api Scope")
                
            
            };

    public static IEnumerable<ApiResource> Apis => new ApiResource[] 
    {
        new ApiResource("companyemployeeapi", "CompanyEmployee API")
        {
                Scopes = { "companyemployeeapi.scope" },
                UserClaims = new List<string> { "role"}
        }
    };

    


    public static IEnumerable<Client> Clients => new Client[]
    {
        new Client
        {
            ClientName = "CompanyEmployeeClient",
            ClientId = "companyemployeeclient",
            AllowedGrantTypes = GrantTypes.Code,
            AccessTokenLifetime = 120,
            AllowOfflineAccess = true,
            UpdateAccessTokenClaimsOnRefresh = true,
            RedirectUris = new List<string>{ "https://localhost:5197/signin-oidc" },
            AllowedScopes = {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.Address, "roles", "companyemployeeapi.scope", "country"},
            ClientSecrets = {new Secret("CompanyEmployeeClientSecret".Sha512())},
            RequirePkce = true,
            RequireConsent = true,
            ClientUri = "https://localhost:5197",
            PostLogoutRedirectUris = new List<string>{ "https://localhost:5197/signout-callback-oidc" }
            

            
        }
    };
}