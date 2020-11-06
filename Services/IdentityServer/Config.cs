// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("remindersmgt", "Reminders Management Service")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "webapi",

                    // no interactive user, use the clientid and secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scope for that client has access to
                    AllowedScopes = { "remindersmgt" }
                }
            };
    }
}