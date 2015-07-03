/* -----------------------------------------------------------------------
 * File: data_schema_0.1_up.sqlserver.sql
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

/* -----------------------------------------------------------------------
 * User authentication cache table
 * -----------------------------------------------------------------------
 */
 CREATE TABLE [dbo].[CacheEntry]
 (
    [Id]           VARCHAR (8000) NOT NULL,
    [Data]         VARCHAR (MAX)  NULL,
    [ExpiryDate]   DATETIMEOFFSET NULL,
    [CreatedDate]  DATETIMEOFFSET NOT NULL,
    [ModifiedDate] DATETIMEOFFSET NOT NULL,
    CONSTRAINT [PK_CacheEntry_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);



/* -----------------------------------------------------------------------
 * User authentication and profile related tables
 * -----------------------------------------------------------------------
 */
CREATE TABLE [dbo].[UserAuth]
(
    [Id]                   INT IDENTITY (1, 1) NOT NULL,
    [UserName]             VARCHAR (255)       NULL,
    [Email]                VARCHAR (255)       NULL,
    [PrimaryEmail]         VARCHAR (255)       NULL,
    [PhoneNumber]          VARCHAR (255)       NULL,
    [FirstName]            NVARCHAR (255)      NULL,
    [LastName]             NVARCHAR (255)      NULL,
    [DisplayName]          NVARCHAR (255)      NULL,
    [Company]              NVARCHAR (255)      NULL,
    [BirthDate]            DATETIME            NULL,
    [BirthDateRaw]         VARCHAR (255)       NULL,
    [Address]              NVARCHAR (MAX)      NULL,
    [Address2]             NVARCHAR (MAX)      NULL,
    [City]                 NVARCHAR (255)      NULL,
    [State]                NVARCHAR (255)      NULL,
    [Country]              NVARCHAR (255)      NULL,
    [Culture]              VARCHAR (255)       NULL,
    [FullName]             NVARCHAR (255)      NULL,
    [Gender]               VARCHAR (255)       NULL,
    [Language]             NVARCHAR (255)      NULL,
    [MailAddress]          NVARCHAR (MAX)      NULL,
    [Nickname]             NVARCHAR (255)      NULL,
    [PostalCode]           VARCHAR (255)       NULL,
    [TimeZone]             VARCHAR (255)       NULL,
    [Salt]                 VARCHAR (255)       NULL,
    [PasswordHash]         VARCHAR (MAX)       NULL,
    [DigestHA1Hash]        VARCHAR (MAX)       NULL,
    [Roles]                VARCHAR (MAX)       NULL,
    [Permissions]          VARCHAR (MAX)       NULL,
    [CreatedDate]          DATETIMEOFFSET      NOT NULL,
    [ModifiedDate]         DATETIMEOFFSET      NOT NULL,
    [InvalidLoginAttempts] INT                 NULL,
    [LastLoginAttempt]     DATETIMEOFFSET      NULL,
    [LockedDate]           DATETIMEOFFSET      NULL,
    [RecoveryToken]        VARCHAR (MAX)       NULL,
    [RefId]                INT                 NULL,
    [RefIdStr]             VARCHAR (MAX)       NULL,
    [Meta]                 VARCHAR (MAX)       NULL,
    CONSTRAINT [PK_UserAuth_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

-- Create index on UserName and Email
CREATE NONCLUSTERED INDEX [IX_UserAuth_UserNameOrEmail]
    ON [dbo].[UserAuth] ([UserName] ASC, [Email] ASC);

CREATE TABLE [dbo].[UserAuthDetails]
(
    [Id]                 INT IDENTITY (1, 1) NOT NULL,
    [UserAuthId]         INT                 NOT NULL,
    [Provider]           VARCHAR (255)       NULL,
    [UserId]             VARCHAR (255)       NULL,
    [UserName]           VARCHAR (255)       NULL,
    [FullName]           NVARCHAR (255)      NULL,
    [DisplayName]        NVARCHAR (255)      NULL,
    [FirstName]          NVARCHAR (255)      NULL,
    [LastName]           NVARCHAR (255)      NULL,
    [Company]            NVARCHAR (255)      NULL,
    [Email]              VARCHAR (255)       NULL,
    [PhoneNumber]        VARCHAR (255)       NULL,
    [BirthDate]          DATETIME            NULL,
    [BirthDateRaw]       VARCHAR (255)       NULL,
    [Address]            NVARCHAR (MAX)      NULL,
    [Address2]           NVARCHAR (MAX)      NULL,
    [City]               NVARCHAR (255)      NULL,
    [State]              NVARCHAR (255)      NULL,
    [Country]            NVARCHAR (255)      NULL,
    [Culture]            VARCHAR (255)       NULL,
    [Gender]             VARCHAR (255)       NULL,
    [Language]           VARCHAR (255)       NULL,
    [MailAddress]        NVARCHAR (MAX)      NULL,
    [Nickname]           NVARCHAR (255)      NULL,
    [PostalCode]         VARCHAR (255)       NULL,
    [TimeZone]           VARCHAR (255)       NULL,
    [RefreshToken]       VARCHAR (MAX)       NULL,
    [RefreshTokenExpiry] DATETIMEOFFSET      NULL,
    [RequestToken]       VARCHAR (MAX)       NULL,
    [RequestTokenSecret] VARCHAR (MAX)       NULL,
    [Items]              VARCHAR (MAX)       NULL,
    [AccessToken]        VARCHAR (MAX)       NULL,
    [AccessTokenSecret]  VARCHAR (MAX)       NULL,
    [CreatedDate]        DATETIMEOFFSET      NOT NULL,
    [ModifiedDate]       DATETIMEOFFSET      NOT NULL,
    [RefId]              INT                 NULL,
    [RefIdStr]           VARCHAR (MAX)       NULL,
    [Meta]               VARCHAR (MAX)       NULL,
    CONSTRAINT [PK_UserAuthDetails_Id]       PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserAuthDetails_UserAuth] FOREIGN KEY ([UserAuthId]) REFERENCES [dbo].[UserAuth]([Id])
);

-- Create index on User Auth FK
CREATE NONCLUSTERED INDEX [IX_UserAuth_UserAuth]
    ON [dbo].[UserAuthDetails] ([UserAuthId] ASC);

CREATE TABLE [dbo].[UserAuthRole]
(
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [UserAuthId]   INT                 NOT NULL,
    [Role]         VARCHAR (MAX)       NULL,
    [Permission]   VARCHAR (MAX)       NULL,
    [CreatedDate]  DATETIMEOFFSET      NOT NULL,
    [ModifiedDate] DATETIMEOFFSET      NOT NULL,
    [RefId]        INT                 NULL,
    [RefIdStr]     VARCHAR (MAX)       NULL,
    [Meta]         VARCHAR (MAX)       NULL,
    CONSTRAINT [PK_UserAuthRole_Id]       PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserAuthRole_UserAuth] FOREIGN KEY ([UserAuthId]) REFERENCES [dbo].[UserAuth]([Id])
);

-- Create index on User Auth FK
CREATE NONCLUSTERED INDEX [IX_UserAuthRole_UserAuth]
    ON [dbo].[UserAuthRole] ([UserAuthId] ASC);