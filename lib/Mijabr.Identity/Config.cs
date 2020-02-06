// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Mijabr.Identity
{
    public static class Config
    {
        private static readonly string Host = System.Environment.GetEnvironmentVariable("host") ?? "localtest.me";

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource("api1", "My API #1"),
                new ApiResource("home", "MIJABR Home"),
                new ApiResource("scrabble", "MIJABR Scrabble")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "home-client",
                    ClientName = "MIJABR Home",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,

                    RedirectUris =
                    {
                        $"http://{Host}/home/redirect",
                        $"https://{Host}/home/redirect"
                    },
                    PostLogoutRedirectUris =
                    {
                        $"http://{Host}/home",
                        $"https://{Host}/home",
                    },
                    AllowedCorsOrigins =
                    {
                        $"http://{Host}",
                        $"https://{Host}"
                    },

                    AllowedScopes = { "openid", "profile", "home" }
                },

                new Client
                {
                    ClientId = "scrabble-client",
                    ClientName = "MIJABR Scrabble",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RequireConsent = false,

                    RedirectUris =
                    {
                        $"http://{Host}/scrabble/redirect",
                        $"https://{Host}/scrabble/redirect"
                    },
                    PostLogoutRedirectUris =
                    {
                        $"http://{Host}/scrabble",
                        $"https://{Host}/scrabble"
                    },
                    AllowedCorsOrigins =
                    {
                        $"http://{Host}",
                        $"https://{Host}"
                    },

                    AllowedScopes = { "openid", "profile", "scrabble" }
                },

                new Client
                {
                ClientId = "mijabr-client",
                ClientName = "MIJABR",
                ClientUri = "http://identityserver.io",

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                RequireConsent = false,

                RedirectUris =
                {
                    $"http://{Host}/home/redirect",
                    $"https://{Host}/home/redirect",
                    $"http://{Host}/scrabble/redirect",
                    $"https://{Host}/scrabble/redirect"
                },
                PostLogoutRedirectUris =
                {
                    $"http://{Host}/home",
                    $"https://{Host}/home",
                    $"http://{Host}/scrabble",
                    $"https://{Host}/scrabble"
                },
                AllowedCorsOrigins =
                {
                    $"http://{Host}",
                    $"https://{Host}"
                },

                AllowedScopes = { "openid", "profile", "scrabble", "home" }
                }
            };
    }
}
//// client credentials flow client
//new Client
//                {
//                    ClientId = "client",
//                    ClientName = "Client Credentials Client",

//                    AllowedGrantTypes = GrantTypes.ClientCredentials,
//                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

//                    AllowedScopes = { "api1" }
//                },

//                // MVC client using code flow + pkce
//                new Client
//                {
//                    ClientId = "mvc",
//                    ClientName = "MVC Client",

//                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
//                    RequirePkce = true,
//                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

//                    RedirectUris = { "http://localhost:5003/signin-oidc" },
//                    FrontChannelLogoutUri = "http://localhost:5003/signout-oidc",
//                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },

//                    AllowOfflineAccess = true,
//                    AllowedScopes = { "openid", "profile", "api1" }
//                },

//                // SPA client using code flow + pkce
//                new Client
//                {
//                    ClientId = "spa",
//                    ClientName = "SPA Client",
//                    ClientUri = "http://identityserver.io",

//                    AllowedGrantTypes = GrantTypes.Code,
//                    RequirePkce = true,
//                    RequireClientSecret = false,

//                    RedirectUris =
//                    {
//                        "http://localhost:5002/index.html",
//                        "http://localhost:5002/callback.html",
//                        "http://localhost:5002/silent.html",
//                        "http://localhost:5002/popup.html",
//                    },

//                    PostLogoutRedirectUris = { "http://localhost:5002/index.html" },
//                    AllowedCorsOrigins = { "http://localhost:5002" },

//                    AllowedScopes = { "openid", "profile", "api1" }
//                },
