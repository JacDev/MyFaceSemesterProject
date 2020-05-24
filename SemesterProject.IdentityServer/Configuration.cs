using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace SemesterProject.IdentityServer
{
	public static class Configuration
	{
		//what client can get from identity server
		public static IEnumerable<IdentityResource> GetIdentityResources() =>
			new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new IdentityResource
				{
					Name = "userinfo",
					UserClaims =
					{
						"FirstName",
						"LastName"
					}
				}
			};

		public static IEnumerable<ApiResource> GetApis() => //jakie sa api i co mogą dostać
			new List<ApiResource>
			{
				new ApiResource("MyFaceApi")
			};
		public static IEnumerable<Client> GetClients() =>
			new List<Client>
			{
				new Client
				{
					ClientId = "MyFaceClient",
					ClientSecrets = { new Secret("MyFaceClientSecret".ToSha256()) },
					AllowedGrantTypes = GrantTypes.Code,

					RedirectUris = { $"{UrlAddressescs.MVCClientUri}signin-oidc" },
					PostLogoutRedirectUris = { $"{UrlAddressescs.MVCClientUri}signout-callback-oidc" },
					FrontChannelLogoutUri= $"{UrlAddressescs.MVCClientUri}signout-oidc" ,

					AllowedScopes = {

						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"MyFaceApi",
						"userinfo"
					},

					AlwaysIncludeUserClaimsInIdToken = true,

					RequireConsent = false
				},
				new Client {
					ClientId = "MyFaceJsClient",

					AllowedGrantTypes = GrantTypes.Code,
					RequirePkce = true,
					RequireClientSecret = false,

					RedirectUris = { "https://localhost:44393/message/jslogin" },
					PostLogoutRedirectUris = { "https://localhost:44393/Test/Index" },
					AllowedCorsOrigins = { "https://localhost:44393" },

					AllowedScopes = {
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"MyFaceApi",
						"userinfo"
					},

					AccessTokenLifetime = 1,

					AllowAccessTokensViaBrowser = true,
					RequireConsent = false,
				},
			};
	}
}
			
