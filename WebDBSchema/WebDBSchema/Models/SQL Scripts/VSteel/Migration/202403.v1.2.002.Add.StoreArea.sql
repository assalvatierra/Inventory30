ALTER TABLE [dbo].[InvItemMasters]
ALTER COLUMN [ItemQty] int

ALTER TABLE [dbo].[InvItemMasters] 
   ADD  [InvStoreAreaId] int  NOT NULL DEFAULT 1 ;
   
-- Creating table 'InvStoreAreas'
CREATE TABLE [dbo].[InvStoreAreas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Remarks] nvarchar(40)  NOT NULL,
    [InvStoreId] int  NOT NULL
);

-- Creating primary key on [Id] in table 'InvStoreAreas'
ALTER TABLE [dbo].[InvStoreAreas]
ADD CONSTRAINT [PK_InvStoreAreas]
    PRIMARY KEY CLUSTERED ([Id] ASC);

    
-- Creating foreign key on [InvStoreAreaId] in table 'InvItemMasters'
ALTER TABLE [dbo].[InvItemMasters]
ADD CONSTRAINT [FK_InvStoreAreaInvItemMaster]
    FOREIGN KEY ([InvStoreAreaId])
    REFERENCES [dbo].[InvStoreAreas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreAreaInvItemMaster'
CREATE INDEX [IX_FK_InvStoreAreaInvItemMaster]
ON [dbo].[InvItemMasters]
    ([InvStoreAreaId]);


-- Creating foreign key on [InvStoreId] in table 'InvStoreAreas'
ALTER TABLE [dbo].[InvStoreAreas]
ADD CONSTRAINT [FK_InvStoreInvStoreArea]
    FOREIGN KEY ([InvStoreId])
    REFERENCES [dbo].[InvStores]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvStoreInvStoreArea'
CREATE INDEX [IX_FK_InvStoreInvStoreArea]
ON [dbo].[InvStoreAreas]
    ([InvStoreId]);

