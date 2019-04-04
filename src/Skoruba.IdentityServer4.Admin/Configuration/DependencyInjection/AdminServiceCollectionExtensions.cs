﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Helpers;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Dtos.Identity;
using Skoruba.IdentityServer4.Admin.BusinessLogic.Identity.Helpers;
using Skoruba.IdentityServer4.Admin.EntityFramework.DbContexts;
using Skoruba.IdentityServer4.Admin.EntityFramework.Identity.Entities.Identity;
using Skoruba.IdentityServer4.Admin.Helpers;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class AdminServiceCollectionExtensions
	{
		/// <summary>
		/// Adds the Skoruba IdentityServer Admin UI with the default Identity model and settings read from 
		/// the configuration and hosting environment.
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <param name="env"></param>
		/// <returns></returns>
		public static IServiceCollection AddIdentityServerAdminUI(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment env)
		{
			string callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
			return AddIdentityServerAdminUI<AdminIdentityDbContext, UserIdentity, UserIdentityRole, UserIdentityUserClaim, UserIdentityUserRole, UserIdentityUserLogin, UserIdentityRoleClaim, UserIdentityUserToken, string>(services, configuration, env, callingAssemblyName);
		}

		/// <summary>
		/// Adds the Skoruba IdentityServer Admin UI with a custom Identity model and settings read from 
		/// the configuration and hosting environment.
		/// </summary>
		/// <typeparam name="TIdentityDbContext"></typeparam>
		/// <typeparam name="TUser"></typeparam>
		/// <typeparam name="TRole"></typeparam>
		/// <typeparam name="TUserClaim"></typeparam>
		/// <typeparam name="TUserRole"></typeparam>
		/// <typeparam name="TUserLogin"></typeparam>
		/// <typeparam name="TRoleClaim"></typeparam>
		/// <typeparam name="TUserToken"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <param name="env"></param>
		/// <returns></returns>
		public static IServiceCollection AddIdentityServerAdminUI<TIdentityDbContext, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TKey>(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment env)
			where TIdentityDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
			where TUser : IdentityUser<TKey>
			where TRole : IdentityRole<TKey>
			where TUserClaim : IdentityUserClaim<TKey>
			where TUserRole : IdentityUserRole<TKey>
			where TUserLogin : IdentityUserLogin<TKey>
			where TRoleClaim : IdentityRoleClaim<TKey>
			where TUserToken : IdentityUserToken<TKey>
			where TKey : IEquatable<TKey>
		{
			string callingAssemblyName = Assembly.GetCallingAssembly().GetName().Name;
			return AddIdentityServerAdminUI<TIdentityDbContext, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TKey>(services, configuration, env, callingAssemblyName);
		}

		private static IServiceCollection AddIdentityServerAdminUI<TIdentityDbContext, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TKey>(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment env, string callingAssemblyName)
			where TIdentityDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
			where TUser : IdentityUser<TKey>
			where TRole : IdentityRole<TKey>
			where TUserClaim : IdentityUserClaim<TKey>
			where TUserRole : IdentityUserRole<TKey>
			where TUserLogin : IdentityUserLogin<TKey>
			where TRoleClaim : IdentityRoleClaim<TKey>
			where TUserToken : IdentityUserToken<TKey>
			where TKey : IEquatable<TKey>
		{
			return AddIdentityServerAdminUI<TIdentityDbContext, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TKey>
				(services, options =>
				{
					// Sets the staging or development settings from the environment's constants.
					options.ApplyHostingEnvironment(env);

					// Applies the provided configuration into the options.
					options.ApplyConfiguration(configuration);
					
					// Sets the migrations assemblies to the calling assembly by default.
					options.ConnectionStrings.SetMigrationsAssemblies(callingAssemblyName);

					// Adds a builder for Serilog to include additional sinks from the provided configuration.
					options.SerilogConfigurationBuilder = serilog => serilog.ReadFrom.Configuration(configuration);
				});
		}

		/// <summary>
		/// Adds the Skoruba IdentityServer Admin UI with a custom Identity model and settings.
		/// </summary>
		/// <typeparam name="TIdentityDbContext"></typeparam>
		/// <typeparam name="TUser"></typeparam>
		/// <param name="services"></param>
		/// <param name="optionsAction"></param>
		/// <returns></returns>
		public static IServiceCollection AddIdentityServerAdminUI<TIdentityDbContext, TUser>(this IServiceCollection services, Action<IdentityServerAdminOptions> optionsAction)
			where TIdentityDbContext : IdentityDbContext<TUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
			where TUser : IdentityUser<string>
			=> AddIdentityServerAdminUI<TIdentityDbContext, TUser, IdentityRole, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>, string>(services, optionsAction);

		/// <summary>
		/// Adds the Skoruba IdentityServer Admin UI with a custom Identity model and settings.
		/// </summary>
		/// <typeparam name="TIdentityDbContext"></typeparam>
		/// <typeparam name="TUser"></typeparam>
		/// <typeparam name="TRole"></typeparam>
		/// <typeparam name="TUserClaim"></typeparam>
		/// <typeparam name="TUserRole"></typeparam>
		/// <typeparam name="TUserLogin"></typeparam>
		/// <typeparam name="TRoleClaim"></typeparam>
		/// <typeparam name="TUserToken"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="services"></param>
		/// <param name="optionsAction"></param>
		/// <returns></returns>
		public static IServiceCollection AddIdentityServerAdminUI<TIdentityDbContext, TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TKey>(this IServiceCollection services, Action<IdentityServerAdminOptions> optionsAction)
			where TIdentityDbContext : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
			where TUser : IdentityUser<TKey>
			where TRole : IdentityRole<TKey>
			where TUserClaim : IdentityUserClaim<TKey>
			where TUserRole : IdentityUserRole<TKey>
			where TUserLogin : IdentityUserLogin<TKey>
			where TRoleClaim : IdentityRoleClaim<TKey>
			where TUserToken : IdentityUserToken<TKey>
			where TKey : IEquatable<TKey>
		{
			// Builds the options from user preferences or configuration.
			IdentityServerAdminOptions options = new IdentityServerAdminOptions(services);
			optionsAction(options);

			// Register configuration from the options.
			services.ConfigureRootConfiguration(options);

			// Add DbContexts for Asp.Net Core Identity, Logging and IdentityServer - Configuration store and Operational store
			services.AddDbContexts<TIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext>(options);

			// Add Asp.Net Core Identity Configuration and OpenIdConnect auth as well
			services.AddAuthenticationServices<TIdentityDbContext, TUser, TRole>(options);

			// Add authorization policies for MVC
			services.AddAuthorizationPolicies();

			// Add exception filters in MVC
			services.AddMvcExceptionFilters();

			// Add all dependencies for IdentityServer Admin
			services.AddAdminServices<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminLogDbContext>();

			// Add all dependencies for Asp.Net Core Identity
			services.AddAdminAspNetIdentityServices<TIdentityDbContext, IdentityServerPersistedGrantDbContext, UserDto<TKey>, TKey, RoleDto<TKey>, TKey, TKey,
				TKey, TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken,
				UsersDto<UserDto<TKey>, TKey>, RolesDto<RoleDto<TKey>, TKey>, UserRolesDto<RoleDto<TKey>, TKey, TKey>,
				UserClaimsDto<TKey>, UserProviderDto<TKey>, UserProvidersDto<TKey>, UserChangePasswordDto<TKey>,
				RoleClaimsDto<TKey>, UserClaimDto<TKey>, RoleClaimDto<TKey>>();

			// Add all dependencies for Asp.Net Core Identity in MVC - these dependencies are injected into generic Controllers
			// Including settings for MVC and Localization
			services.AddMvcWithLocalization<UserDto<TKey>, TKey, RoleDto<TKey>, TKey, TKey, TKey,
				TUser, TRole, TKey, TUserClaim, TUserRole,
				TUserLogin, TRoleClaim, TUserToken,
				UsersDto<UserDto<TKey>, TKey>, RolesDto<RoleDto<TKey>, TKey>, UserRolesDto<RoleDto<TKey>, TKey, TKey>,
				UserClaimsDto<TKey>, UserProviderDto<TKey>, UserProvidersDto<TKey>, UserChangePasswordDto<TKey>,
				RoleClaimsDto<TKey>>();

			return services;
		}
	}
}