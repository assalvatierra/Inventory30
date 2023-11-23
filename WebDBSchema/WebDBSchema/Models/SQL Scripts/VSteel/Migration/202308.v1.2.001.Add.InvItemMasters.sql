
--- Create New Tables ----

-- Creating table 'InvItemMasters'
CREATE TABLE [dbo].[InvItemMasters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [LotNo] nvarchar(20)  NULL,
    [BatchNo] nvarchar(20)  NOT NULL,
    [ItemQty] nvarchar(max)  NOT NULL,
    [InvUomId] int  NOT NULL,
    [Remarks] nvarchar(80)  NOT NULL,
    [InvItemBrandId] int  NOT NULL,
    [InvItemOriginId] int  NOT NULL
);


-- Creating table 'InvTrxDtlxItemMasters'
CREATE TABLE [dbo].[InvTrxDtlxItemMasters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvTrxDtlId] int  NOT NULL,
    [InvItemMasterId] int  NOT NULL
);


-- Creating table 'InvItemBrands'
CREATE TABLE [dbo].[InvItemBrands] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Code] nvarchar(20)  NULL
);


-- Creating table 'InvItemOrigins'
CREATE TABLE [dbo].[InvItemOrigins] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Code] nvarchar(20)  NULL
);




--- Create Primary Keys ----

-- Creating primary key on [Id] in table 'InvItemMasters'
ALTER TABLE [dbo].[InvItemMasters]
ADD CONSTRAINT [PK_InvItemMasters]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvTrxDtlxItemMasters'
ALTER TABLE [dbo].[InvTrxDtlxItemMasters]
ADD CONSTRAINT [PK_InvTrxDtlxItemMasters]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvItemBrands'
ALTER TABLE [dbo].[InvItemBrands]
ADD CONSTRAINT [PK_InvItemBrands]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvItemOrigins'
ALTER TABLE [dbo].[InvItemOrigins]
ADD CONSTRAINT [PK_InvItemOrigins]
    PRIMARY KEY CLUSTERED ([Id] ASC);


--- Create Foreign Keys ----

-- Creating foreign key on [InvItemId] in table 'InvItemMasters'
ALTER TABLE [dbo].[InvItemMasters]
ADD CONSTRAINT [FK_InvItemInvItemMaster]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemMaster'
CREATE INDEX [IX_FK_InvItemInvItemMaster]
ON [dbo].[InvItemMasters]
    ([InvItemId]);
GO

-- Creating foreign key on [InvUomId] in table 'InvItemMasters'
ALTER TABLE [dbo].[InvItemMasters]
ADD CONSTRAINT [FK_InvUomInvItemMaster]
    FOREIGN KEY ([InvUomId])
    REFERENCES [dbo].[InvUoms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvUomInvItemMaster'
CREATE INDEX [IX_FK_InvUomInvItemMaster]
ON [dbo].[InvItemMasters]
    ([InvUomId]);
GO

-- Creating foreign key on [InvTrxDtlId] in table 'InvTrxDtlxItemMasters'
ALTER TABLE [dbo].[InvTrxDtlxItemMasters]
ADD CONSTRAINT [FK_InvTrxDtlInvTrxDtlxItemMaster]
    FOREIGN KEY ([InvTrxDtlId])
    REFERENCES [dbo].[InvTrxDtls]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxDtlInvTrxDtlxItemMaster'
CREATE INDEX [IX_FK_InvTrxDtlInvTrxDtlxItemMaster]
ON [dbo].[InvTrxDtlxItemMasters]
    ([InvTrxDtlId]);
GO

-- Creating foreign key on [InvItemMasterId] in table 'InvTrxDtlxItemMasters'
ALTER TABLE [dbo].[InvTrxDtlxItemMasters]
ADD CONSTRAINT [FK_InvItemMasterInvTrxDtlxItemMaster]
    FOREIGN KEY ([InvItemMasterId])
    REFERENCES [dbo].[InvItemMasters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemMasterInvTrxDtlxItemMaster'
CREATE INDEX [IX_FK_InvItemMasterInvTrxDtlxItemMaster]
ON [dbo].[InvTrxDtlxItemMasters]
    ([InvItemMasterId]);
GO

-- Creating foreign key on [InvItemBrandId] in table 'InvItemMasters'
ALTER TABLE [dbo].[InvItemMasters]
ADD CONSTRAINT [FK_InvItemBrandInvItemMaster]
    FOREIGN KEY ([InvItemBrandId])
    REFERENCES [dbo].[InvItemBrands]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemBrandInvItemMaster'
CREATE INDEX [IX_FK_InvItemBrandInvItemMaster]
ON [dbo].[InvItemMasters]
    ([InvItemBrandId]);
GO

-- Creating foreign key on [InvItemOriginId] in table 'InvItemMasters'
ALTER TABLE [dbo].[InvItemMasters]
ADD CONSTRAINT [FK_InvItemOriginInvItemMaster]
    FOREIGN KEY ([InvItemOriginId])
    REFERENCES [dbo].[InvItemOrigins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemOriginInvItemMaster'
CREATE INDEX [IX_FK_InvItemOriginInvItemMaster]
ON [dbo].[InvItemMasters]
    ([InvItemOriginId]);
GO
