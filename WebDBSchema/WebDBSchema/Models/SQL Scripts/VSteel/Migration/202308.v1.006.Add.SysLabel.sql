-- Creating table 'SysLabels'
CREATE TABLE [dbo].[SysLabels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [DisplayText] nvarchar(50)  NOT NULL,
    [DisplayLang] nvarchar(2)  NOT NULL
);


-- Creating table 'SysSettings'
CREATE TABLE [dbo].[SysSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(10)  NOT NULL,
    [SysValue] nvarchar(255)  NOT NULL
);

    
-- Creating primary key on [Id] in table 'SysLabels'
ALTER TABLE [dbo].[SysLabels]
ADD CONSTRAINT [PK_SysLabels]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'SysSettings'
ALTER TABLE [dbo].[SysSettings]
ADD CONSTRAINT [PK_SysSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);