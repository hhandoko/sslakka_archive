// -----------------------------------------------------------------------
// <copyright file="UserSession.cs" company="Sslakka">
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

namespace Sslakka.Data.SchemaMigration
{
    using System;
    using System.Reflection;

    using FluentMigrator;
    using FluentMigrator.Runner;
    using FluentMigrator.Runner.Announcers;
    using FluentMigrator.Runner.Initialization;
    using FluentMigrator.Runner.Processors;

    using ServiceStack.Logging;

    /// <summary>
    /// The schema synchronization FluentMigrator runner.
    /// </summary>
    public class SchemaSync
    {
        /// <summary>
        /// The default schema type.
        /// </summary>
        private const string DefaultSchemaType = "data";

        /// <summary>
        /// The default database type.
        /// </summary>
        private const string DefaultDbType = "sqlserver";

        /// <summary>
        /// The schema type.
        /// </summary>
        private readonly string schemaType;

        /// <summary>
        /// The database type.
        /// </summary>
        private readonly string dbType;

        /// <summary>
        /// The database connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILog Log { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSync"/> class.
        /// </summary>
        /// <param name="connectionString">The SQL Server database connection string.</param>
        public SchemaSync(string connectionString)
            : this(connectionString, DefaultSchemaType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSync"/> class.
        /// </summary>
        /// <param name="connectionString">The SQL Server database connection string.</param>
        /// <param name="schemaType">The schema type.</param>
        public SchemaSync(string connectionString, string schemaType)
            : this(connectionString, schemaType, DefaultDbType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaSync"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="schemaType">The schema type.</param>
        /// <param name="dbType">The database type.</param>
        public SchemaSync(string connectionString, string schemaType, string dbType)
        {
            this.connectionString = connectionString;
            this.schemaType = schemaType;
            this.dbType = dbType;
            Log = LogManager.GetLogger(GetType());
        }

        /// <summary>
        /// The schema sync main method.
        /// </summary>
        /// <param name="runnerAction">The runner action.</param>
        // TODO: Update table and column names to lowercase
        public void Sync(Action<IMigrationRunner> runnerAction)
        {
            // Announcer
            var announcer = new TextWriterAnnouncer(x => Log.Debug(x));

            // Assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Migration context
            var context = new RunnerContext(announcer) { ApplicationContext = schemaType };

            // Processor
            var factory = new MigrationProcessorFactoryProvider().GetFactory(dbType);
            var options = new SchemaSyncOptions { PreviewOnly = false, Timeout = 0 };
            var processor = factory.Create(connectionString, announcer, options);

            // Runner
            var runner = new MigrationRunner(assembly, context, processor);

            try
            {
                runnerAction(runner);
            }
            catch (Exception ex)
            {
                Log.Error("[migration]::[runner]::[sync] Unexpected error during data schema sync", ex);
            }
        }

        /// <summary>
        /// The schema sync runner options.
        /// </summary>
        private class SchemaSyncOptions
            : IMigrationProcessorOptions
        {
            /// <summary>
            /// Gets or sets a value indicating whether migration task is for preview only.
            /// </summary>
            public bool PreviewOnly { get; set; }

            /// <summary>
            /// Gets or sets the timeout in milliseconds.
            /// </summary>
            public int Timeout { get; set; }

            /// <summary>
            /// Gets or sets the provider switches.
            /// </summary>
            public string ProviderSwitches { get; set; }
        }
    }
}
