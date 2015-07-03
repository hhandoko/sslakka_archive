/* -----------------------------------------------------------------------
 * File: system_schema_0.1_dn.sqlserver.sql
 *
 *   Copyright (c) 2015 Sslakka and its contributors
 *
 *   Licensed under the Apache License, Version 2.0 (the "License");
 *   you may not use this file except in compliance with the License.
 *   You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *   Unless required by applicable law or agreed to in writing, software
 *   distributed under the License is distributed on an "AS IS" BASIS,
 *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *   See the License for the specific language governing permissions and
 *   limitations under the License.
 *
 * -----------------------------------------------------------------------
 */
IF OBJECT_ID('ELMAH_GetErrorsXml', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[ELMAH_GetErrorsXml];

 IF OBJECT_ID('ELMAH_GetErrorXml', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[ELMAH_GetErrorXml];

IF OBJECT_ID('ELMAH_LogError', 'P') IS NOT NULL
    DROP PROCEDURE [dbo].[ELMAH_LogError];

 IF OBJECT_ID('ELMAH_Error', 'U') IS NOT NULL
    DROP TABLE [dbo].[ELMAH_Error];