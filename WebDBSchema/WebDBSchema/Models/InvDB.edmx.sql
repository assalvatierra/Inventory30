
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/03/2021 11:56:19
-- Generated from EDMX file: C:\Users\Acer-PC\Documents\GitHub\Inventory20\WebDBSchema\WebDBSchema\Models\InvDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\ACER-PC\DOCUMENTS\GITHUB\INVENTORY20\DBFILES\ASPNET-WEBINV.SERVER-8C0EE0EB-8AC1-4CFF-924C-24E20C1606AB.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_InvItemInvItemClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItemClasses] DROP CONSTRAINT [FK_InvItemInvItemClass];
GO
IF OBJECT_ID(N'[dbo].[FK_InvClassificationInvItemClass]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItemClasses] DROP CONSTRAINT [FK_InvClassificationInvItemClass];
GO
IF OBJECT_ID(N'[dbo].[FK_InvSupplierInvSupplierItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvSupplierItems] DROP CONSTRAINT [FK_InvSupplierInvSupplierItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvSupplierItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvSupplierItems] DROP CONSTRAINT [FK_InvItemInvSupplierItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvSupplierInvPoHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvPoHdrs] DROP CONSTRAINT [FK_InvSupplierInvPoHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_InvPoHdrInvPoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvPoItems] DROP CONSTRAINT [FK_InvPoHdrInvPoItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvPoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvPoItems] DROP CONSTRAINT [FK_InvItemInvPoItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvStoreInvRecHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRecHdrs] DROP CONSTRAINT [FK_InvStoreInvRecHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvRecItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRecItems] DROP CONSTRAINT [FK_InvItemInvRecItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvRecHdrInvRecItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRecItems] DROP CONSTRAINT [FK_InvRecHdrInvRecItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvSupplierInvRecHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRecHdrs] DROP CONSTRAINT [FK_InvSupplierInvRecHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_InvStoreInvRequestHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRequestHdrs] DROP CONSTRAINT [FK_InvStoreInvRequestHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvRequestItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRequestItems] DROP CONSTRAINT [FK_InvItemInvRequestItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvRequestHdrInvRequestItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRequestItems] DROP CONSTRAINT [FK_InvRequestHdrInvRequestItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvStoreInvPoHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvPoHdrs] DROP CONSTRAINT [FK_InvStoreInvPoHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_InvStoreInvAdjHdr]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvAdjHdrs] DROP CONSTRAINT [FK_InvStoreInvAdjHdr];
GO
IF OBJECT_ID(N'[dbo].[FK_InvAdjHdrInvAdjItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvAdjItems] DROP CONSTRAINT [FK_InvAdjHdrInvAdjItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvItemInvAdjItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvAdjItems] DROP CONSTRAINT [FK_InvItemInvAdjItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvUomInvItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItems] DROP CONSTRAINT [FK_InvUomInvItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvUomInvPoItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvPoItems] DROP CONSTRAINT [FK_InvUomInvPoItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvUomInvRecItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRecItems] DROP CONSTRAINT [FK_InvUomInvRecItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvUomInvRequestItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRequestItems] DROP CONSTRAINT [FK_InvUomInvRequestItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvUomInvAdjItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvAdjItems] DROP CONSTRAINT [FK_InvUomInvAdjItem];
GO
IF OBJECT_ID(N'[dbo].[FK_InvRecItemInvRequestItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvRequestItems] DROP CONSTRAINT [FK_InvRecItemInvRequestItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[InvItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvItems];
GO
IF OBJECT_ID(N'[dbo].[InvItemClasses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvItemClasses];
GO
IF OBJECT_ID(N'[dbo].[InvClassifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvClassifications];
GO
IF OBJECT_ID(N'[dbo].[InvSuppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvSuppliers];
GO
IF OBJECT_ID(N'[dbo].[InvSupplierItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvSupplierItems];
GO
IF OBJECT_ID(N'[dbo].[InvStores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvStores];
GO
IF OBJECT_ID(N'[dbo].[InvPoHdrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvPoHdrs];
GO
IF OBJECT_ID(N'[dbo].[InvPoItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvPoItems];
GO
IF OBJECT_ID(N'[dbo].[InvRecHdrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvRecHdrs];
GO
IF OBJECT_ID(N'[dbo].[InvRecItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvRecItems];
GO
IF OBJECT_ID(N'[dbo].[InvRequestHdrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvRequestHdrs];
GO
IF OBJECT_ID(N'[dbo].[InvRequestItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvRequestItems];
GO
IF OBJECT_ID(N'[dbo].[InvAdjHdrs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvAdjHdrs];
GO
IF OBJECT_ID(N'[dbo].[InvAdjItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvAdjItems];
GO
IF OBJECT_ID(N'[dbo].[InvUoms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvUoms];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'InvItems'
CREATE TABLE [dbo].[InvItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL,
    [InvUomId] int  NOT NULL
);
GO

-- Creating table 'InvItemClasses'
CREATE TABLE [dbo].[InvItemClasses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvClassificationId] int  NOT NULL
);
GO

-- Creating table 'InvClassifications'
CREATE TABLE [dbo].[InvClassifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Classification] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvSuppliers'
CREATE TABLE [dbo].[InvSuppliers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NULL
);
GO

-- Creating table 'InvSupplierItems'
CREATE TABLE [dbo].[InvSupplierItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvSupplierId] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [Remarks] nvarchar(max)  NULL,
    [Price] decimal(18,0)  NOT NULL,
    [LastUpdate] datetime  NOT NULL,
    [LeadTime] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvStores'
CREATE TABLE [dbo].[InvStores] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StoreName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvPoHdrs'
CREATE TABLE [dbo].[InvPoHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvSupplierId] int  NOT NULL,
    [InvStoreId] int  NOT NULL,
    [DtPo] datetime  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvPoItems'
CREATE TABLE [dbo].[InvPoItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvPoHdrId] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL,
    [InvUomId] int  NOT NULL
);
GO

-- Creating table 'InvRecHdrs'
CREATE TABLE [dbo].[InvRecHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [InvSupplierId] int  NOT NULL,
    [DtRec] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvRecItems'
CREATE TABLE [dbo].[InvRecItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvRecHdrId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvRequestHdrs'
CREATE TABLE [dbo].[InvRequestHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [DtRec] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvRequestItems'
CREATE TABLE [dbo].[InvRequestItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvRequestHdrId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL,
    [InvRecItemId] int  NOT NULL
);
GO

-- Creating table 'InvAdjHdrs'
CREATE TABLE [dbo].[InvAdjHdrs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvStoreId] int  NOT NULL,
    [dtAdj] nvarchar(max)  NOT NULL,
    [Status] nvarchar(max)  NOT NULL,
    [Remarks] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvAdjItems'
CREATE TABLE [dbo].[InvAdjItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvAdjHdrId] int  NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvUomId] int  NOT NULL,
    [QtyAdded] nvarchar(max)  NOT NULL,
    [QtyDeduct] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvUoms'
CREATE TABLE [dbo].[InvUoms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [uom] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [PK_InvItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvItemClasses'
ALTER TABLE [dbo].[InvItemClasses]
ADD CONSTRAINT [PK_InvItemClasses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvClassifications'
ALTER TABLE [dbo].[InvClassifications]
ADD CONSTRAINT [PK_InvClassifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvSuppliers'
ALTER TABLE [dbo].[InvSuppliers]
ADD CONSTRAINT [PK_InvSuppliers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvSupplierItems'
ALTER TABLE [dbo].[InvSupplierItems]
ADD CONSTRAINT [PK_InvSupplierItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvStores'
ALTER TABLE [dbo].[InvStores]
ADD CONSTRAINT [PK_InvStores]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [PK_InvPoHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [PK_InvPoItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRecHdrs'
ALTER TABLE [dbo].[InvRecHdrs]
ADD CONSTRAINT [PK_InvRecHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [PK_InvRecItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRequestHdrs'
ALTER TABLE [dbo].[InvRequestHdrs]
ADD CONSTRAINT [PK_InvRequestHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [PK_InvRequestItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvAdjHdrs'
ALTER TABLE [dbo].[InvAdjHdrs]
ADD CONSTRAINT [PK_InvAdjHdrs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [PK_InvAdjItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvUoms'
ALTER TABLE [dbo].[InvUoms]
ADD CONSTRAINT [PK_InvUoms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [InvItemId] in table 'InvItemClasses'
ALTER TABLE [dbo].[InvItemClasses]
ADD CONSTRAINT [FK_InvItemInvItemClass]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemClass'
CREATE INDEX [IX_FK_InvItemInvItemClass]
ON [dbo].[InvItemClasses]
    ([InvItemId]);
GO

-- Creating foreign key on [InvClassificationId] in table 'InvItemClasses'
ALTER TABLE [dbo].[InvItemClasses]
ADD CONSTRAINT [FK_InvClassificationInvItemClass]
    FOREIGN KEY ([InvClassificationId])
    REFERENCES [dbo].[InvClassifications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvClassificationInvItemClass'
CREATE INDEX [IX_FK_InvClassificationInvItemClass]
ON [dbo].[InvItemClasses]
    ([InvClassificationId]);
GO

-- Creating foreign key on [InvSupplierId] in table 'InvSupplierItems'
ALTER TABLE [dbo].[InvSupplierItems]
ADD CONSTRAINT [FK_InvSupplierInvSupplierItem]
    FOREIGN KEY ([InvSupplierId])
    REFERENCES [dbo].[InvSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvSupplierInvSupplierItem'
CREATE INDEX [IX_FK_InvSupplierInvSupplierItem]
ON [dbo].[InvSupplierItems]
    ([InvSupplierId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvSupplierItems'
ALTER TABLE [dbo].[InvSupplierItems]
ADD CONSTRAINT [FK_InvItemInvSupplierItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvSupplierItem'
CREATE INDEX [IX_FK_InvItemInvSupplierItem]
ON [dbo].[InvSupplierItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvSupplierId] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [FK_InvSupplierInvPoHdr]
    FOREIGN KEY ([InvSupplierId])
    REFERENCES [dbo].[InvSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvSupplierInvPoHdr'
CREATE INDEX [IX_FK_InvSupplierInvPoHdr]
ON [dbo].[InvPoHdrs]
    ([InvSupplierId]);
GO

-- Creating foreign key on [InvPoHdrId] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [FK_InvPoHdrInvPoItem]
    FOREIGN KEY ([InvPoHdrId])
    REFERENCES [dbo].[InvPoHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvPoHdrInvPoItem'
CREATE INDEX [IX_FK_InvPoHdrInvPoItem]
ON [dbo].[InvPoItems]
    ([InvPoHdrId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [FK_InvItemInvPoItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvPoItem'
CREATE INDEX [IX_FK_InvItemInvPoItem]
ON [dbo].[InvPoItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvRecHdrs'
ALTER TABLE [dbo].[InvRecHdrs]
ADD CONSTRAINT [FK_InvStoreInvRecHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvRecHdr'
CREATE INDEX [IX_FK_InvStoreInvRecHdr]
ON [dbo].[InvRecHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [FK_InvItemInvRecItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvRecItem'
CREATE INDEX [IX_FK_InvItemInvRecItem]
ON [dbo].[InvRecItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvRecHdrId] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [FK_InvRecHdrInvRecItem]
    FOREIGN KEY ([InvRecHdrId])
    REFERENCES [dbo].[InvRecHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvRecHdrInvRecItem'
CREATE INDEX [IX_FK_InvRecHdrInvRecItem]
ON [dbo].[InvRecItems]
    ([InvRecHdrId]);
GO

-- Creating foreign key on [InvSupplierId] in table 'InvRecHdrs'
ALTER TABLE [dbo].[InvRecHdrs]
ADD CONSTRAINT [FK_InvSupplierInvRecHdr]
    FOREIGN KEY ([InvSupplierId])
    REFERENCES [dbo].[InvSuppliers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvSupplierInvRecHdr'
CREATE INDEX [IX_FK_InvSupplierInvRecHdr]
ON [dbo].[InvRecHdrs]
    ([InvSupplierId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvRequestHdrs'
ALTER TABLE [dbo].[InvRequestHdrs]
ADD CONSTRAINT [FK_InvStoreInvRequestHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvRequestHdr'
CREATE INDEX [IX_FK_InvStoreInvRequestHdr]
ON [dbo].[InvRequestHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvItemInvRequestItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvRequestItem'
CREATE INDEX [IX_FK_InvItemInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvRequestHdrId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvRequestHdrInvRequestItem]
    FOREIGN KEY ([InvRequestHdrId])
    REFERENCES [dbo].[InvRequestHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvRequestHdrInvRequestItem'
CREATE INDEX [IX_FK_InvRequestHdrInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvRequestHdrId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvPoHdrs'
ALTER TABLE [dbo].[InvPoHdrs]
ADD CONSTRAINT [FK_InvStoreInvPoHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvPoHdr'
CREATE INDEX [IX_FK_InvStoreInvPoHdr]
ON [dbo].[InvPoHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvStoreId] in table 'InvAdjHdrs'
ALTER TABLE [dbo].[InvAdjHdrs]
ADD CONSTRAINT [FK_InvStoreInvAdjHdr]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvAdjHdr'
CREATE INDEX [IX_FK_InvStoreInvAdjHdr]
ON [dbo].[InvAdjHdrs]
    ([InvStoreId]);
GO

-- Creating foreign key on [InvAdjHdrId] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [FK_InvAdjHdrInvAdjItem]
    FOREIGN KEY ([InvAdjHdrId])
    REFERENCES [dbo].[InvAdjHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvAdjHdrInvAdjItem'
CREATE INDEX [IX_FK_InvAdjHdrInvAdjItem]
ON [dbo].[InvAdjItems]
    ([InvAdjHdrId]);
GO

-- Creating foreign key on [InvItemId] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [FK_InvItemInvAdjItem]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvAdjItem'
CREATE INDEX [IX_FK_InvItemInvAdjItem]
ON [dbo].[InvAdjItems]
    ([InvItemId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvItems'
ALTER TABLE [dbo].[InvItems]
ADD CONSTRAINT [FK_InvUomInvItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvItem'
CREATE INDEX [IX_FK_InvUomInvItem]
ON [dbo].[InvItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvPoItems'
ALTER TABLE [dbo].[InvPoItems]
ADD CONSTRAINT [FK_InvUomInvPoItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvPoItem'
CREATE INDEX [IX_FK_InvUomInvPoItem]
ON [dbo].[InvPoItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvRecItems'
ALTER TABLE [dbo].[InvRecItems]
ADD CONSTRAINT [FK_InvUomInvRecItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvRecItem'
CREATE INDEX [IX_FK_InvUomInvRecItem]
ON [dbo].[InvRecItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvUomInvRequestItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvRequestItem'
CREATE INDEX [IX_FK_InvUomInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvAdjItems'
ALTER TABLE [dbo].[InvAdjItems]
ADD CONSTRAINT [FK_InvUomInvAdjItem]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvAdjItem'
CREATE INDEX [IX_FK_InvUomInvAdjItem]
ON [dbo].[InvAdjItems]
    ([InvUomId]);
GO

-- Creating foreign key on [InvRecItemId] in table 'InvRequestItems'
ALTER TABLE [dbo].[InvRequestItems]
ADD CONSTRAINT [FK_InvRecItemInvRequestItem]
    FOREIGN KEY ([InvRecItemId])
    REFERENCES [dbo].[InvRecItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvRecItemInvRequestItem'
CREATE INDEX [IX_FK_InvRecItemInvRequestItem]
ON [dbo].[InvRequestItems]
    ([InvRecItemId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------