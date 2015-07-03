/* -----------------------------------------------------------------------
 * File: data_schema_0.1_dn.sqlserver.sql
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
IF OBJECT_ID('Message', 'U') IS NOT NULL
    DROP TABLE [dbo].[Message];

IF OBJECT_ID('ChannelMember', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChannelMember];

IF OBJECT_ID('Channel', 'U') IS NOT NULL
    DROP TABLE [dbo].[Channel];

IF OBJECT_ID('GroupHistory', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupHistory];

IF OBJECT_ID('GroupMember', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupMember];

IF OBJECT_ID('Group', 'U') IS NOT NULL
    DROP TABLE [dbo].[Group];

 IF OBJECT_ID('UserVerification', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserVerification];

 IF OBJECT_ID('UserAuthRole', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAuthRole];

IF OBJECT_ID('UserAuthDetails', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAuthDetails];

IF OBJECT_ID('UserAuth', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAuth];

IF OBJECT_ID('CacheEntry', 'U') IS NOT NULL
    DROP TABLE [dbo].[CacheEntry];