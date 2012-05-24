
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/18/2012 20:39:28
-- Generated from EDMX file: C:\Users\KwanieMacbook\Desktop\AlfrescoWorklog\AlfrescoWorklog\AlfrescoWorklog\Models\AlfrescoWorklogDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AlfrescoWorklogDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Project_Sandpit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_Project_Sandpit];
GO
IF OBJECT_ID(N'[dbo].[FK_Project_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_Project_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[Sandpits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sandpits];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [name] varchar(40)  NOT NULL,
    [description] varchar(200)  NOT NULL,
    [startTime] datetime  NOT NULL,
    [endTime] datetime  NOT NULL,
    [windowsLogon] varchar(50)  NOT NULL,
    [userID] int  NOT NULL,
    [sandpitID] int  NOT NULL,
    [isDeployed] bit  NOT NULL
);
GO

-- Creating table 'Sandpits'
CREATE TABLE [dbo].[Sandpits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [name] varchar(50)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [name] varchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Sandpits'
ALTER TABLE [dbo].[Sandpits]
ADD CONSTRAINT [PK_Sandpits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [sandpitID] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_Project_Sandpit]
    FOREIGN KEY ([sandpitID])
    REFERENCES [dbo].[Sandpits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Project_Sandpit'
CREATE INDEX [IX_FK_Project_Sandpit]
ON [dbo].[Projects]
    ([sandpitID]);
GO

-- Creating foreign key on [userID] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [FK_Project_User]
    FOREIGN KEY ([userID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Project_User'
CREATE INDEX [IX_FK_Project_User]
ON [dbo].[Projects]
    ([userID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------