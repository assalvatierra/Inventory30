
-- --------------------------------------------------
-- Added 'InvCustomSpecs', 'InvItemCustomSpecs', 'InvCatCustomSpecs' &'InvCustomSpecTypes' Tables
-- --------------------------------------------------
-- Date Created: 10/08/2022 10:40:30
-- Generated from EDMX file: C:\DATA\GitHub\Inventory30\WebDBSchema\WebDBSchema\Models\InvDB.edmx
-- --------------------------------------------------

-- Creating table 'InvCustomSpecs'
CREATE TABLE [dbo].[InvCustomSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SpecName] nvarchar(50)  NOT NULL,
    [InvCustomSpecTypeId] int  NOT NULL,
    [Order] int  NOT NULL,
    [Measurement] nvarchar(20)  NULL,
    [Remarks] nvarchar(50)  NULL
);


-- Creating table 'InvItemCustomSpecs'
CREATE TABLE [dbo].[InvItemCustomSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [InvCustomSpecId] int  NOT NULL,
    [SpecValue] nvarchar(30)  NULL,
    [Remarks] nvarchar(50)  NULL,
    [Order] nvarchar(max)  NOT NULL
);


-- Creating table 'InvCatCustomSpecs'
CREATE TABLE [dbo].[InvCatCustomSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvCategoryId] int  NOT NULL,
    [Order] int  NOT NULL,
    [Remarks] nvarchar(50)  NULL,
    [InvCustomSpecId] int  NOT NULL
);


-- Creating table 'InvCustomSpecTypes'
CREATE TABLE [dbo].[InvCustomSpecTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);


-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'InvCustomSpecs'
ALTER TABLE [dbo].[InvCustomSpecs]
ADD CONSTRAINT [PK_InvCustomSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvItemCustomSpecs'
ALTER TABLE [dbo].[InvItemCustomSpecs]
ADD CONSTRAINT [PK_InvItemCustomSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvCatCustomSpecs'
ALTER TABLE [dbo].[InvCatCustomSpecs]
ADD CONSTRAINT [PK_InvCatCustomSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvCustomSpecTypes'
ALTER TABLE [dbo].[InvCustomSpecTypes]
ADD CONSTRAINT [PK_InvCustomSpecTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [InvCategoryId] in table 'InvCatCustomSpecs'
ALTER TABLE [dbo].[InvCatCustomSpecs]
ADD CONSTRAINT [FK_InvCategoryInvCatCustomSpec]
    FOREIGN KEY ([InvCategoryId])
    REFERENCES [dbo].[InvCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvCategoryInvCatCustomSpec'
CREATE INDEX [IX_FK_InvCategoryInvCatCustomSpec]
ON [dbo].[InvCatCustomSpecs]
    ([InvCategoryId]);


-- Creating foreign key on [InvItemCustomSpecId] in table 'InvCatCustomSpecs'
ALTER TABLE [dbo].[InvCatCustomSpecs]
ADD CONSTRAINT [FK_InvCustomSpecInvCatCustomSpec]
    FOREIGN KEY ([InvCustomSpecId])
    REFERENCES [dbo].[InvCustomSpecs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InvCustomSpecInvCatCustomSpec'
CREATE INDEX [IX_FK_InvCustomSpecInvCatCustomSpec]
ON [dbo].[InvCatCustomSpecs]
    ([InvCustomSpecId]);



-- Creating foreign key on [InvCustomSpecTypeId] in table 'InvCustomSpecs'
ALTER TABLE [dbo].[InvCustomSpecs]
ADD CONSTRAINT [FK_InvCustomSpecTypeInvCustomSpec]
    FOREIGN KEY ([InvCustomSpecTypeId])
    REFERENCES [dbo].[InvCustomSpecTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvCustomSpecTypeInvCustomSpec'
CREATE INDEX [IX_FK_InvCustomSpecTypeInvCustomSpec]
ON [dbo].[InvCustomSpecs]
    ([InvCustomSpecTypeId]);

-- Creating foreign key on [InvItemId] in table 'InvItemCustomSpecs'
ALTER TABLE [dbo].[InvItemCustomSpecs]
ADD CONSTRAINT [FK_InvItemInvItemCustomSpec]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemCustomSpec'
CREATE INDEX [IX_FK_InvItemInvItemCustomSpec]
ON [dbo].[InvItemCustomSpecs]
    ([InvItemId]);


-- Creating foreign key on [InvCustomSpecId] in table 'InvItemCustomSpecs'
ALTER TABLE [dbo].[InvItemCustomSpecs]
ADD CONSTRAINT [FK_InvCustomSpecInvItemCustomSpec]
    FOREIGN KEY ([InvCustomSpecId])
    REFERENCES [dbo].[InvCustomSpecs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvCustomSpecInvItemCustomSpec'
CREATE INDEX [IX_FK_InvCustomSpecInvItemCustomSpec]
ON [dbo].[InvItemCustomSpecs]
    ([InvCustomSpecId]);


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------