

-- Change LotNo datatype from Int to varchar(20)
ALTER TABLE [dbo].[InvTrxDtls]
ALTER COLUMN [LotNo] nvarchar(20) NULL;



ALTER TABLE [dbo].[InvTrxDtls]
 ADD [Remarks] nvarchar(80)  NULL;