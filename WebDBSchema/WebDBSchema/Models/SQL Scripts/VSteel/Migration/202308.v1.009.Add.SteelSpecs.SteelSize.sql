

CREATE TABLE [dbo].[SteelSizes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL,
    [Code] nvarchar(10)  NULL
);

ALTER TABLE [dbo].[InvItemSpec_Steel] 
   ADD  
   [SteelSizeId] int  NOT NULL DEFAULT 1 ;
   


-- Creating primary key on [Id] in table 'SteelSizes'
ALTER TABLE [dbo].[SteelSizes]
ADD CONSTRAINT [PK_SteelSizes]
    PRIMARY KEY CLUSTERED ([Id] ASC);


    
-- Creating foreign key on [SteelSizeId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelSizeInvItemSpec_Steel]
    FOREIGN KEY ([SteelSizeId])
    REFERENCES [dbo].[SteelSizes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_SteelSizeInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelSizeInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelSizeId]);

