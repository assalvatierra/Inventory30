

CREATE TABLE [dbo].[RptUserLevels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AspNetUserId] nvarchar(255)  NOT NULL,
    [RptRole] nvarchar(max)  NOT NULL
);

ALTER TABLE [dbo].[RptUserLevels]
ADD CONSTRAINT [PK_RptUserLevels]
    PRIMARY KEY CLUSTERED ([Id] ASC);