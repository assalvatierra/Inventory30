

ALTER TABLE [dbo].[InvTrxHdrs]
   ADD [Party] nvarchar(80) NULL;

   

ALTER TABLE [dbo].[InvTrxApprovals]
   ADD [ApprovedAccBy] nvarchar(80) NULL;
   
ALTER TABLE [dbo].[InvTrxApprovals]
   ADD [ApprovedAccDate] date NULL;