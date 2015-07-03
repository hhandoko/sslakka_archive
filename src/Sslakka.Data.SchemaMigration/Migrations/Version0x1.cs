// -----------------------------------------------------------------------
// <copyright file="Version0x1.cs" company="Sslakka">
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
    using FluentMigrator;

    /// <summary>
    /// The database schema sync v0.1.
    /// </summary>
    /// <remarks>
    /// Schema version convention based on Semantic Versioning Specification,
    /// ({MAJOR}.{MINOR}.{PATCH}):
    ///   * MAJOR are 10,000s
    ///   * MINOR are 100s
    ///   * PATCH are 1s
    /// 
    /// For example, schema `21201` corresponds to v2.12.1
    /// 
    /// See:
    ///   * http://semver.org/
    /// </remarks>
    [Migration(100, "Initial database schema.")]
    public class Version0x1 : Migration
    {
        /// <summary>
        /// The up-versioning method.
        /// </summary>
        public override void Up()
        {
            IfDatabase("sqlserver")
                .Execute.EmbeddedScript(
                    (string)ApplicationContext == "system"
                        ? "system_schema_0.1_up.sqlserver.sql"
                        : "data_schema_0.1_up.sqlserver.sql");
        }

        /// <summary>
        /// The down-versioning method.
        /// </summary>
        public override void Down()
        {
            IfDatabase("sqlserver")
                .Execute.EmbeddedScript(
                    (string)ApplicationContext == "system"
                        ? "system_schema_0.1_dn.sqlserver.sql"
                        : "data_schema_0.1_dn.sqlserver.sql");
        }
    }
}
