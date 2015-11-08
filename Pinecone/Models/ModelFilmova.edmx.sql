
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/08/2015 21:10:12
-- Generated from EDMX file: d:\nikol\documents\visual studio 2015\Projects\Pinecone\Pinecone\Models\ModelFilmova.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BazaFilmova];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FilmsGlumacFilm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GlumacFilmSet] DROP CONSTRAINT [FK_FilmsGlumacFilm];
GO
IF OBJECT_ID(N'[dbo].[FK_FilmsZanrFilm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ZanrFilmSet] DROP CONSTRAINT [FK_FilmsZanrFilm];
GO
IF OBJECT_ID(N'[dbo].[FK_GlumacsGlumacFilm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GlumacFilmSet] DROP CONSTRAINT [FK_GlumacsGlumacFilm];
GO
IF OBJECT_ID(N'[dbo].[FK_ZanrsZanrFilm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ZanrFilmSet] DROP CONSTRAINT [FK_ZanrsZanrFilm];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FilmsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FilmsSet];
GO
IF OBJECT_ID(N'[dbo].[GlumacFilmSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GlumacFilmSet];
GO
IF OBJECT_ID(N'[dbo].[GlumacsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GlumacsSet];
GO
IF OBJECT_ID(N'[dbo].[ZanrFilmSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZanrFilmSet];
GO
IF OBJECT_ID(N'[dbo].[ZanrsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ZanrsSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'FilmsSet'
CREATE TABLE [dbo].[FilmsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Naslov] nvarchar(max)  NOT NULL,
    [Radnja] nvarchar(max)  NOT NULL,
    [Ocjena] decimal(18,0)  NOT NULL,
    [Trajanje] smallint  NOT NULL
);
GO

-- Creating table 'ZanrsSet'
CREATE TABLE [dbo].[ZanrsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Zanr] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GlumacsSet'
CREATE TABLE [dbo].[GlumacsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Ime] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GlumacFilmSet'
CREATE TABLE [dbo].[GlumacFilmSet] (
    [Film_Id] int  NOT NULL,
    [Glumac_Id] int  NOT NULL
);
GO

-- Creating table 'ZanrFilmSet'
CREATE TABLE [dbo].[ZanrFilmSet] (
    [Film_Id] int  NOT NULL,
    [Zanr_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'FilmsSet'
ALTER TABLE [dbo].[FilmsSet]
ADD CONSTRAINT [PK_FilmsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ZanrsSet'
ALTER TABLE [dbo].[ZanrsSet]
ADD CONSTRAINT [PK_ZanrsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GlumacsSet'
ALTER TABLE [dbo].[GlumacsSet]
ADD CONSTRAINT [PK_GlumacsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Film_Id], [Glumac_Id] in table 'GlumacFilmSet'
ALTER TABLE [dbo].[GlumacFilmSet]
ADD CONSTRAINT [PK_GlumacFilmSet]
    PRIMARY KEY CLUSTERED ([Film_Id], [Glumac_Id] ASC);
GO

-- Creating primary key on [Film_Id], [Zanr_Id] in table 'ZanrFilmSet'
ALTER TABLE [dbo].[ZanrFilmSet]
ADD CONSTRAINT [PK_ZanrFilmSet]
    PRIMARY KEY CLUSTERED ([Film_Id], [Zanr_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Film_Id] in table 'ZanrFilmSet'
ALTER TABLE [dbo].[ZanrFilmSet]
ADD CONSTRAINT [FK_FilmsZanrFilm]
    FOREIGN KEY ([Film_Id])
    REFERENCES [dbo].[FilmsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Zanr_Id] in table 'ZanrFilmSet'
ALTER TABLE [dbo].[ZanrFilmSet]
ADD CONSTRAINT [FK_ZanrsZanrFilm]
    FOREIGN KEY ([Zanr_Id])
    REFERENCES [dbo].[ZanrsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ZanrsZanrFilm'
CREATE INDEX [IX_FK_ZanrsZanrFilm]
ON [dbo].[ZanrFilmSet]
    ([Zanr_Id]);
GO

-- Creating foreign key on [Film_Id] in table 'GlumacFilmSet'
ALTER TABLE [dbo].[GlumacFilmSet]
ADD CONSTRAINT [FK_FilmsGlumacFilm]
    FOREIGN KEY ([Film_Id])
    REFERENCES [dbo].[FilmsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Glumac_Id] in table 'GlumacFilmSet'
ALTER TABLE [dbo].[GlumacFilmSet]
ADD CONSTRAINT [FK_GlumacsGlumacFilm]
    FOREIGN KEY ([Glumac_Id])
    REFERENCES [dbo].[GlumacsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GlumacsGlumacFilm'
CREATE INDEX [IX_FK_GlumacsGlumacFilm]
ON [dbo].[GlumacFilmSet]
    ([Glumac_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------