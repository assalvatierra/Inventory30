
-- Add Batch No on InvTrxDtls
-- Add [Weight] and [Material] on InvItems
-- Date: 01/27/2023

ALTER TABLE InvTrxDtls
	ADD [BatchNo] varchar(40) NULL;

	
ALTER TABLE InvItems
	ADD  [Weight] decimal(18,2)  NULL,
         [Material] nvarchar(80)  NULL
	;