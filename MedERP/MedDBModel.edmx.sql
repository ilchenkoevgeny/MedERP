
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/11/2020 14:24:26
-- Generated from EDMX file: C:\Users\user\source\repos\MedWPF\DB\MedERP\MedERP\MedDBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MedDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientsVisits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitsSet] DROP CONSTRAINT [FK_ClientsVisits];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSessionsVisits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitsSet] DROP CONSTRAINT [FK_UserSessionsVisits];
GO
IF OBJECT_ID(N'[dbo].[FK_VisitTypeVisits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitsSet] DROP CONSTRAINT [FK_VisitTypeVisits];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UsersSet] DROP CONSTRAINT [FK_UsersRoles];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UsersSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersSet];
GO
IF OBJECT_ID(N'[dbo].[UserSessionsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSessionsSet];
GO
IF OBJECT_ID(N'[dbo].[RolesSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolesSet];
GO
IF OBJECT_ID(N'[dbo].[ClientsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientsSet];
GO
IF OBJECT_ID(N'[dbo].[VisitsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VisitsSet];
GO
IF OBJECT_ID(N'[dbo].[VisitTypeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VisitTypeSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [RoleID] int  NOT NULL,
    [Firstname] nvarchar(50)  NULL,
    [Lastname] nvarchar(50)  NOT NULL,
    [Middlename] nvarchar(50)  NULL,
    [Phone] nvarchar(25)  NULL,
    [Dismissed] bit  NOT NULL,
    [Roles_RoleID] int  NOT NULL
);
GO

-- Creating table 'UserSessionsSet'
CREATE TABLE [dbo].[UserSessionsSet] (
    [SessionID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [StartDateTime] datetime  NOT NULL,
    [EndDateTime] datetime  NOT NULL
);
GO

-- Creating table 'RolesSet'
CREATE TABLE [dbo].[RolesSet] (
    [RoleID] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL,
    [RemoveVisits] bit  NOT NULL,
    [RemoveClients] bit  NOT NULL,
    [AddUsers] bit  NOT NULL
);
GO

-- Creating table 'ClientsSet'
CREATE TABLE [dbo].[ClientsSet] (
    [ClientID] int IDENTITY(1,1) NOT NULL,
    [Firstname] nvarchar(50)  NULL,
    [Lastname] nvarchar(50)  NULL,
    [Middlename] nvarchar(50)  NULL,
    [Photo] varbinary(max)  NULL,
    [Gender] nvarchar(15)  NOT NULL,
    [BirthDate] datetime  NULL,
    [Address] nvarchar(255)  NULL,
    [Phone] nvarchar(25)  NULL,
    [AddedByUser] int  NOT NULL
);
GO

-- Creating table 'VisitsSet'
CREATE TABLE [dbo].[VisitsSet] (
    [VisitID] int IDENTITY(1,1) NOT NULL,
    [ClientID] int  NOT NULL,
    [UserID] int  NOT NULL,
    [SessionID] int  NOT NULL,
    [TypeID] int  NOT NULL,
    [Diagnosis] nvarchar(max)  NULL,
    [VisitDateTime] datetime  NOT NULL,
    [Removed] bit  NOT NULL,
    [RemovedByUser] int  NOT NULL
);
GO

-- Creating table 'VisitTypeSet'
CREATE TABLE [dbo].[VisitTypeSet] (
    [TypeID] int IDENTITY(1,1) NOT NULL,
    [TypeName] nvarchar(25)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [UserID] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [PK_UsersSet]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- Creating primary key on [SessionID] in table 'UserSessionsSet'
ALTER TABLE [dbo].[UserSessionsSet]
ADD CONSTRAINT [PK_UserSessionsSet]
    PRIMARY KEY CLUSTERED ([SessionID] ASC);
GO

-- Creating primary key on [RoleID] in table 'RolesSet'
ALTER TABLE [dbo].[RolesSet]
ADD CONSTRAINT [PK_RolesSet]
    PRIMARY KEY CLUSTERED ([RoleID] ASC);
GO

-- Creating primary key on [ClientID] in table 'ClientsSet'
ALTER TABLE [dbo].[ClientsSet]
ADD CONSTRAINT [PK_ClientsSet]
    PRIMARY KEY CLUSTERED ([ClientID] ASC);
GO

-- Creating primary key on [VisitID] in table 'VisitsSet'
ALTER TABLE [dbo].[VisitsSet]
ADD CONSTRAINT [PK_VisitsSet]
    PRIMARY KEY CLUSTERED ([VisitID] ASC);
GO

-- Creating primary key on [TypeID] in table 'VisitTypeSet'
ALTER TABLE [dbo].[VisitTypeSet]
ADD CONSTRAINT [PK_VisitTypeSet]
    PRIMARY KEY CLUSTERED ([TypeID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClientID] in table 'VisitsSet'
ALTER TABLE [dbo].[VisitsSet]
ADD CONSTRAINT [FK_ClientsVisits]
    FOREIGN KEY ([ClientID])
    REFERENCES [dbo].[ClientsSet]
        ([ClientID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientsVisits'
CREATE INDEX [IX_FK_ClientsVisits]
ON [dbo].[VisitsSet]
    ([ClientID]);
GO

-- Creating foreign key on [SessionID] in table 'VisitsSet'
ALTER TABLE [dbo].[VisitsSet]
ADD CONSTRAINT [FK_UserSessionsVisits]
    FOREIGN KEY ([SessionID])
    REFERENCES [dbo].[UserSessionsSet]
        ([SessionID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSessionsVisits'
CREATE INDEX [IX_FK_UserSessionsVisits]
ON [dbo].[VisitsSet]
    ([SessionID]);
GO

-- Creating foreign key on [TypeID] in table 'VisitsSet'
ALTER TABLE [dbo].[VisitsSet]
ADD CONSTRAINT [FK_VisitTypeVisits]
    FOREIGN KEY ([TypeID])
    REFERENCES [dbo].[VisitTypeSet]
        ([TypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VisitTypeVisits'
CREATE INDEX [IX_FK_VisitTypeVisits]
ON [dbo].[VisitsSet]
    ([TypeID]);
GO

-- Creating foreign key on [Roles_RoleID] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [FK_UsersRoles]
    FOREIGN KEY ([Roles_RoleID])
    REFERENCES [dbo].[RolesSet]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersRoles'
CREATE INDEX [IX_FK_UsersRoles]
ON [dbo].[UsersSet]
    ([Roles_RoleID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------