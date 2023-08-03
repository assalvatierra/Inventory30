

ALTER TABLE [dbo].[InvItemSpec_Steel]
 DROP COLUMN [SpecFor] ,
    [SizeValue],
    [SizeDesc] ,
    [WtValue]  ,
    [WtDesc]   ,
    [SpecInfo] ;


ALTER TABLE [dbo].[InvItemSpec_Steel] 
   ADD  
    [SteelMainCatId] int ,
    [SteelSubCatId] int  ,
    [SteelBrandId] int   ,
    [SteelMaterialId] int,
    [SteelOriginId] int  ,
    [SteelMaterialGradeId] int,
    [WtKgm] decimal(18,0) ,
    [WtKgpc] decimal(18,0) ;


-- Creating table 'SteelMainCats'
CREATE TABLE [dbo].[SteelMainCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelSubCats'
CREATE TABLE [dbo].[SteelSubCats] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelBrands'
CREATE TABLE [dbo].[SteelBrands] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelMaterials'
CREATE TABLE [dbo].[SteelMaterials] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(80)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelOrigins'
CREATE TABLE [dbo].[SteelOrigins] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO

-- Creating table 'SteelMaterialGrades'
CREATE TABLE [dbo].[SteelMaterialGrades] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(60)  NOT NULL,
    [Code] nvarchar(20)  NULL
);
GO



-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelMainCats'
ALTER TABLE [dbo].[SteelMainCats]
ADD CONSTRAINT [PK_SteelMainCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelSubCats'
ALTER TABLE [dbo].[SteelSubCats]
ADD CONSTRAINT [PK_SteelSubCats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelBrands'
ALTER TABLE [dbo].[SteelBrands]
ADD CONSTRAINT [PK_SteelBrands]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelMaterials'
ALTER TABLE [dbo].[SteelMaterials]
ADD CONSTRAINT [PK_SteelMaterials]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteelOrigins'
ALTER TABLE [dbo].[SteelOrigins]
ADD CONSTRAINT [PK_SteelOrigins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SteeelMaterialGrades'
ALTER TABLE [dbo].[SteelMaterialGrades]
ADD CONSTRAINT [PK_SteelMaterialGrades]
    PRIMARY KEY CLUSTERED ([Id] ASC);



-- Creating foreign key on [ReportId] in table 'RptReportUsers'
ALTER TABLE [dbo].[RptReportUsers]
ADD CONSTRAINT [FK_ReportRptReportUser]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[Reports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportRptReportUser'
CREATE INDEX [IX_FK_ReportRptReportUser]
ON [dbo].[RptReportUsers]
    ([ReportId]);
GO

-- Creating foreign key on [ReportId] in table 'RptReportRoles1'
ALTER TABLE [dbo].[RptReportRoles1]
ADD CONSTRAINT [FK_ReportRptReportRoles]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[Reports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportRptReportRoles'
CREATE INDEX [IX_FK_ReportRptReportRoles]
ON [dbo].[RptReportRoles1]
    ([ReportId]);
GO

-- Creating foreign key on [ReportId] in table 'RptReportCats'
ALTER TABLE [dbo].[RptReportCats]
ADD CONSTRAINT [FK_ReportRptReportCat]
    FOREIGN KEY ([ReportId])
    REFERENCES [dbo].[Reports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportRptReportCat'
CREATE INDEX [IX_FK_ReportRptReportCat]
ON [dbo].[RptReportCats]
    ([ReportId]);
GO

-- Creating foreign key on [SteelMainCatId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelMainCatInvItemSpec_Steel]
    FOREIGN KEY ([SteelMainCatId])
    REFERENCES [dbo].[SteelMainCats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelMainCatInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelMainCatInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelMainCatId]);
GO

-- Creating foreign key on [SteelSubCatId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelSubCatInvItemSpec_Steel]
    FOREIGN KEY ([SteelSubCatId])
    REFERENCES [dbo].[SteelSubCats]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelSubCatInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelSubCatInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelSubCatId]);
GO

-- Creating foreign key on [SteelBrandId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelBrandInvItemSpec_Steel]
    FOREIGN KEY ([SteelBrandId])
    REFERENCES [dbo].[SteelBrands]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelBrandInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelBrandInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelBrandId]);
GO

-- Creating foreign key on [SteelMaterialId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelMaterialInvItemSpec_Steel]
    FOREIGN KEY ([SteelMaterialId])
    REFERENCES [dbo].[SteelMaterials]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelMaterialInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelMaterialInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelMaterialId]);
GO

-- Creating foreign key on [SteelOriginId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelOriginInvItemSpec_Steel]
    FOREIGN KEY ([SteelOriginId])
    REFERENCES [dbo].[SteelOrigins]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelOriginInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelOriginInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelOriginId]);
GO

-- Creating foreign key on [SteelMaterialGradeId] in table 'InvItemSpec_Steel'
ALTER TABLE [dbo].[InvItemSpec_Steel]
ADD CONSTRAINT [FK_SteelMaterialGradeInvItemSpec_Steel]
    FOREIGN KEY ([SteelMaterialGradeId])
    REFERENCES [dbo].[SteelMaterialGrades]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SteelMaterialGradeInvItemSpec_Steel'
CREATE INDEX [IX_FK_SteelMaterialGradeInvItemSpec_Steel]
ON [dbo].[InvItemSpec_Steel]
    ([SteelMaterialGradeId]);
GO
