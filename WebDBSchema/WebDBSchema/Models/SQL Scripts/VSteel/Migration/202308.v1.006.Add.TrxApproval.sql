

-- Creating table 'InvTrxApprovals'
CREATE TABLE [dbo].[InvTrxApprovals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApprovedBy] nvarchar(max)  NOT NULL,
    [ApprovedDate] nvarchar(max)  NOT NULL,
    [VerifiedBy] nvarchar(max)  NOT NULL,
    [VerifiedDate] nvarchar(max)  NOT NULL,
    [EncodedBy] nvarchar(max)  NOT NULL,
    [EncodedDate] nvarchar(max)  NOT NULL,
    [InvTrxHdrId] int  NOT NULL
);

-- Creating table 'InvPOApprovals'
CREATE TABLE [dbo].[InvPoApprovals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ApprovedBy] nvarchar(40)  NOT NULL,
    [ApprovedDate] datetime  NOT NULL,
    [VerifiedBy] nvarchar(40)  NOT NULL,
    [VerifiedDate] datetime  NOT NULL,
    [EncodedBy] nvarchar(40)  NOT NULL,
    [EncodedDate] datetime  NOT NULL,
    [InvPoHdrId] int  NOT NULL
);


-- Creating primary key on [Id] in table 'InvTrxApprovals'
ALTER TABLE [dbo].[InvTrxApprovals]
ADD CONSTRAINT [PK_InvTrxApprovals]
    PRIMARY KEY CLUSTERED ([Id] ASC);


-- Creating primary key on [Id] in table 'InvPOApprovals'
ALTER TABLE [dbo].[InvPoApprovals]
ADD CONSTRAINT [PK_InvPoApprovals]
    PRIMARY KEY CLUSTERED ([Id] ASC);


    
-- Creating foreign key on [InvTrxHdrId] in table 'InvTrxApprovals'
ALTER TABLE [dbo].[InvTrxApprovals]
ADD CONSTRAINT [FK_InvTrxHdrInvTrxApproval]
    FOREIGN KEY ([InvTrxHdrId])
    REFERENCES [dbo].[InvTrxHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvTrxHdrInvTrxApproval'
CREATE INDEX [IX_FK_InvTrxHdrInvTrxApproval]
ON [dbo].[InvTrxApprovals]
    ([InvTrxHdrId]);


    
-- Creating foreign key on [InvPoHdrId] in table 'InvPoApprovals'
ALTER TABLE [dbo].[InvPoApprovals]
ADD CONSTRAINT [FK_InvPoHdrInvPoApproval]
    FOREIGN KEY ([InvPoHdrId])
    REFERENCES [dbo].[InvPoHdrs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;


-- Creating non-clustered index for FOREIGN KEY 'FK_InvPoHdrInvPoApproval'
CREATE INDEX [IX_FK_InvPoHdrInvPoApproval]
ON [dbo].[InvPoApprovals]
    ([InvPoHdrId]);
