using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
				//new ApiResource("MyFaceApi", new [] {ClaimTypes.NameIdentifier, "UserInfo" })
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

					//add refresh_token
					//AllowOfflineAccess = true,

					//add page between client and identityserver to user can check what claims can by pass by idenserv
					RequireConsent = false
				}
			};
	}
}
