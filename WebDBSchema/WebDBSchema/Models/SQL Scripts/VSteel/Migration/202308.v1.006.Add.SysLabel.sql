-- Creating table 'SysLabels'
CREATE TABLE [dbo].[SysLabels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [DisplayText] nvarchar(50)  NOT NULL,
    [DisplayLang] nvarchar(2)  NOT NULL
);


-- Creating primary key on [Id] in table 'SysLabels'
ALTER TABLE [dbo].[SysLabels]
ADD CONSTRAINT [PK_SysLabels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
