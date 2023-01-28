
-- Add Batch No and Weight on InvTrxDtls
-- Date: 01/27/2023

ALTER TABLE InvTrxDtls
	ADD [BatchNo] varchar(40) NULL,
	    [Material] varchar(80)  NULL;