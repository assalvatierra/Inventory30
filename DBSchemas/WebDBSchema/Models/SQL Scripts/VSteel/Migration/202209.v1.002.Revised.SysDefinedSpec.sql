-- Update InvItemSysDefinedSpecs
-- Add new table [InvCategorySpecDefs]
-- Date: 10/1/2022

-- drop foreign key on SysDefinedSpec
IF OBJECT_ID(N'[dbo].[FK_InvCategoryInvItemSysDefinedSpecs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvItemSysDefinedSpecs] DROP CONSTRAINT [FK_InvCategoryInvItemSysDefinedSpecs];


-- drop index 
IF OBJECT_ID(N'[dbo].[IX_FK_InvCategoryInvItemSysDefinedSpecs]', 'F') IS NOT NULL
DROP INDEX [InvItemSysDefinedSpecs].[IX_FK_InvCategoryInvItemSysDefinedSpecs]; 

-- drop column InvCategoryId
ALTER TABLE  [dbo].[InvItemSysDefinedSpecs]
DROP COLUMN InvCategoryId;

-- Creating table 'InvCategorySpecDefs'
CREATE TABLE [dbo].[InvCategorySpecDefs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [InvCategoryId] int  NOT NULL,
    [InvItemSysDefinedSpecsId] int  NOT NULL
);


-- Creating primary key on [Id] in table 'InvItemSysDefinedSpecs'
ALTER TABLE [dbo].[InvItemSysDefinedSpecs]
ADD CONSTRAINT [PK_InvItemSysDefinedSpecs]
    PRIMARY KEY CLUSTERED ([Id] ASC);

    
-- Creating foreign key on [InvItemSysDefinedSpecsId] in table 'InvCategorySpecDefs'
ALTER TABLE [dbo].[InvCategorySpecDefs]
ADD CONSTRAINT [FK_InvItemSysDefinedSpecsInvCategorySpecDef]
    FOREIGN KEY ([InvItemSysDefinedSpecsId])
    REFERENCES [dbo].[InvItemSysDefinedSpecs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvItemSysDefinedSpecsInvCategorySpecDef'
CREATE INDEX [IX_FK_InvItemSysDefinedSpecsInvCategorySpecDef]
ON [dbo].[InvCategorySpecDefs]
    ([InvItemSysDefinedSpecsId]);