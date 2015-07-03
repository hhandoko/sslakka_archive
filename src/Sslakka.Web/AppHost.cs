// -----------------------------------------------------------------------
// <copyright file="AppHost.cs" company="Sslakka">
//   Copyright (c) 2015 Sslakka and its contributors
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
// -----------------------------------------------------------------------

namespace Sslakka.Web
{
    using System;
    using System.Collections.Generic;

    using Funq;

    using ServiceStack;
    using ServiceStack.Api.Swagger;
    using ServiceStack.Auth;
    using ServiceStack.Caching;
    using ServiceStack.Configuration;
    using ServiceStack.Data;
    using ServiceStack.MiniProfiler;
    using ServiceStack.MiniProfiler.Data;
    using ServiceStack.OrmLite;
    using ServiceStack.OrmLite.SqlServer;
    using ServiceStack.Razor;
    using ServiceStack.Text;
    using ServiceStack.Validation;

    using Sslakka.Data.SchemaMigration;
    using Sslakka.Service;

    /// <summary>
    /// The Web Application host.
    /// </summary>
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// The application config parameters.
        /// </summary>
        public static AppConfig AppConfig;

        /// <summary>
        /// The application settings.
        /// </summary>
        public static AppSettings AppSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppHost"/> class.
        /// </summary>
        public AppHost()
            : base("Sslakka", typeof(AppHost).Assembly)
        {
        }

        /// <summary>
        /// Configure the Web Application host.
        /// </summary>
        /// <param name="container">The IoC container.</param>
        public override void Configure(Container container)
        {
            // Configure ServiceStack host
            ConfigureHost(container);

            // Configure JSON serialization properties
            ConfigureSerialization(container);

            // Configure application settings and configuration parameters
            ConfigureApplication(container);

            // Configure database schema synchronization
            ConfigureDataSchema(container);

            // Configure ServiceStack database connections
            ConfigureDataConnection(container);

            // Configure caching mechanism
            ConfigureCache(container);

            // Configure ServiceStack Authentication plugin
            ConfigureUserAuth(container);

            // Configure ServiceStack Fluent Validation plugin
            ConfigureValidation(container);

            // Configure ServiceStack Razor views
            ConfigureView(container);

            // Configure various system tools / features
            ConfigureTools(container);
        }

        /// <summary>
        /// Configure ServiceStack host.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureHost(Container container)
        {
            SetConfig(new HostConfig
            {
                AppendUtf8CharsetOnContentTypes = new HashSet<string> { MimeTypes.Html },

                // Set to return JSON if no request content type is defined
                // e.g. text/html or application/json
                DefaultContentType = MimeTypes.Json,
#if DEBUG
                // Show StackTraces in service responses during development
                DebugMode = true,
#endif
                // Disable SOAP endpoints
                EnableFeatures = Feature.All.Remove(Feature.Soap)
            });
        }

        /// <summary>
        /// Configure JSON serialization properties.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureSerialization(Container container)
        {
            // Set JSON web services to return idiomatic JSON camelCase properties
            JsConfig.EmitCamelCaseNames = true;

            // Set JSON web services to return ISO8601 date format
            JsConfig.DateHandler = DateHandler.ISO8601;

            // Exclude type info during serialization,
            // except for UserSession DTO
            JsConfig.ExcludeTypeInfo = true;
            JsConfig<UserSession>.IncludeTypeInfo = true;
        }

        /// <summary>
        /// Configure application settings and configuration.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureApplication(Container container)
        {
            // Set application settings
            AppSettings = new AppSettings();
            container.Register<IAppSettings>(AppSettings);

            // Set configuration parameters
            AppConfig = new AppConfig(AppSettings);
            // TODO: Inject AppConfig
        }

        /// <summary>
        /// Configure database schema synchronization.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureDataSchema(Container container)
        {
            var dataSchemaSync = new SchemaSync(ConfigUtils.GetConnectionString("Data"), "data");
            var systemSchemaSync = new SchemaSync(ConfigUtils.GetConnectionString("System"), "system");
#if DEBUG
            systemSchemaSync.Sync(x => x.MigrateDown(0));
            dataSchemaSync.Sync(x => x.MigrateDown(0));
#endif
            dataSchemaSync.Sync(x => x.MigrateUp());
            systemSchemaSync.Sync(x => x.MigrateUp());
        }

        /// <summary>
        /// Configure database connections.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureDataConnection(Container container)
        {
            // Get connection string
            var dbConnStr = AppSettings.Get(
                "SQLSERVER_CONNECTION_STRING",
                ConfigUtils.GetConnectionString("Data"));

            // Configure database connection settings
            var dbConn =
                new OrmLiteConnectionFactory(
                    dbConnStr,
                    SqlServerOrmLiteDialectProvider.Instance)
                {
#if DEBUG
                    ConnectionFilter = x => new ProfiledDbConnection(x, Profiler.Current)
#endif
                };

            // Register database connection factory
            container.Register<IDbConnectionFactory>(dbConn);

            // Register repositories

            // NOTE: Service Interfaces are auto-injected by ServiceStack
        }

        /// <summary>
        /// Configure caching mechanism
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureCache(Container container)
        {
            // User OrmLite SQL database-backed persistent cache
            container.RegisterAs<OrmLiteCacheClient, ICacheClient>();
        }

        /// <summary>
        /// Configure ServiceStack Authentication plugin.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureUserAuth(Container container)
        {
            // Override default authentication routes
            var authRoutes =
                new Dictionary<Type, string[]>
                    {
                        { typeof(AuthenticateService), new [] { "/auth", "/auth/{provider}" } },
                        { typeof(AssignRolesService), new [] { "/roles/assign" } },
                        { typeof(UnAssignRolesService), new [] { "/roles/unassign" } }
                    };

            // Register all applicable authentication provider(s) for this web app
            Plugins.Add(
                new AuthFeature(() =>
                    new UserSession(),
                    new IAuthProvider[]
                        {
                            new CredentialsAuthProvider(AppSettings)
                        })
                {
                    ServiceRoutes = authRoutes,
                    IncludeRegistrationService = true
                });

            // Store user data into the referenced SQL Server database,
            // use OrmLite DB Connection to persist the UserAuth and AuthProvider info
            container.Register<IUserAuthRepository>(c =>
                new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>())
                {
                    UseDistinctRoleTables = true
                });
        }

        /// <summary>
        /// Configure ServiceStack Fluent Validation plugin.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureValidation(Container container)
        {
            // Provide fluent validation functionality for web services
            Plugins.Add(new ValidationFeature());

            // TODO: Add validator's assembly for scanning
        }

        /// <summary>
        /// Configure ServiceStack Razor views.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureView(Container container)
        {
            // Enable ServiceStack Razor
            Plugins.Add(new RazorFormat());
        }

        /// <summary>
        /// Configure various system tools / features.
        /// </summary>
        /// <param name="container">The DI / IoC container.</param>
        private void ConfigureTools(Container container)
        {
            // Add Postman and Swagger UI support
            Plugins.Add(new PostmanFeature());
            Plugins.Add(new SwaggerFeature());

            // Add CORS (Cross-Origin Resource Sharing) support
            Plugins.Add(new CorsFeature());
#if DEBUG
            // Development-time features
            // Add request logger
            // See: https://github.com/ServiceStack/ServiceStack/wiki/Request-logger
            Plugins.Add(new RequestLogsFeature());
#endif
        }
    }
}