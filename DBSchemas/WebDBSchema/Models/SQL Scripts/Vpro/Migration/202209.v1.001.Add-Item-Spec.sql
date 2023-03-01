-- ADD InvItemSpec_Steel and InvItemSysDefinedSpecs
-- 09/28/2022
-- Last Update on 01/07/2023

-- Creating table 'InvItemSpec_Steel'
CREATE TABLE [dbo].[InvItemSpec_Steel] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvItemId] int  NOT NULL,
    [SpecFor] nvarchar(10)  NOT NULL,
    [SizeValue] nvarchar(10)  NULL,
    [SizeDesc] nvarchar(30)  NULL,
    [WtValue] nvarchar(10)  NULL,
    [WtDesc] nvarchar(30)  NULL,
    [SpecInfo] nvarchar(80)  NULL
);


-- Creating table 'InvItemSysDefinedSpecs'
CREATE TABLE [dbo].[InvItemSysDefinedSpecs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SpecName] nvarchar(30)  NULL,
    [SpecCode] nvarchar(10)  NOT NULL,
    [SpecGroup] nvarchar(10)  NULL,
    [InvCategoryId] int  NOT NULL
);



-- Creating primary key on [Id] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [PK_InvItemSpec_Steel]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvItemSysDefinedSpecs'
ALTER TABLE [dbo].[InvItemSysDefinedSpecs]
ADD CONSTRAINT [PK_InvItemSysDefinedSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);




-- Creating foreign key on [InvCategoryId] in table 'InvItemSysDefinedSpecs'
ALTER TABLE [dbo].[InvItemSysDefinedSpecs]
ADD CONSTRAINT [FK_InvCategoryInvItemSysDefinedSpecs]
    FOREIGN KEY ([InvCategoryId])
    REFERENCES [dbo].[InvCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvCategoryInvItemSysDefinedSpecs'
CREATE INDEX [IX_FK_InvCategoryInvItemSysDefinedSpecs]
ON [dbo].[InvItemSysDefinedSpecs]
    ([InvCategoryId]);


-- Creating foreign key on [InvItemId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_InvItemInvItemSpec_Steel]
    FOREIGN KEY ([InvItemId])
    REFERENCES [dbo].[InvItems]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemInvItemSpec_Steel'
CREATE INDEX [IX_FK_InvItemInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([InvItemId]);

