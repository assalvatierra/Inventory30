

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(250)  NULL,
    [DisplayName] varchar(250)  NULL,
    [LayoutData] varbinary(max)  NULL
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
