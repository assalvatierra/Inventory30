DECLARE @Today AS DATETIME
SET @Today = GetDate()

--Sample Items --
IF NOT EXISTS (SELECT 1 FROM InvItems)
BEGIN
	insert into InvItems([Code],[Description],[Remarks],[InvUomId]) values
	('ITEM01','SAMPLE ITEM', '', 1)
	;
END

--Sample Suppliers --
IF NOT EXISTS (SELECT 1 FROM InvSuppliers)
BEGIN
	insert into InvSuppliers([Name],[Remarks]) values
	('Vsteel - Davao','Davao');
END

-- Add items to suppliers
IF NOT EXISTS (SELECT 1 FROM InvSupplierItems)
BEGIN
	insert into InvSupplierItems([InvSupplierId],[InvItemId],[Remarks],[Price],[LastUpdate],[LeadTime],[UserId]) values
	(1,1,'',50, @Today ,'30 Days','admin@gmail.com');
END

-- Store 1 : Transaction Headers --
insert into InvTrxHdrs([InvStoreId],[DtTrx],[UserId],[Remarks],[InvTrxTypeId],[InvTrxHdrStatusId]) values
(1,@Today,'admin@gmail.com','',1,1);

--Store 1: Transaction Detalis --
insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values 
(1, 1, 1, 1, 1);

-- Store 2 : Transaction Headers --
insert into InvTrxHdrs([InvStoreId],[DtTrx],[UserId],[Remarks],[InvTrxTypeId],[InvTrxHdrStatusId]) values
(1,@Today,'admin@gmail.com','',1,1);

--Store 2: Transaction Detalis --
insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values
(1, 1, 1, 1, 1);

--Inv Classifications --
insert into InvItemClasses ([InvItemId],[InvClassificationId]) values
(1, 1);

--Inv Items Warning Levels --
insert into InvWarningLevels([InvItemId],[Level1],[Level2],[InvWarningTypeId],[InvUomId]) values
(1, 11, 15, 1, 1);


--Inv Uom Conversions
insert into InvUomConversions([InvUomId_base],[InvUomId_into],[Factor],[Description]) values
(1, 2, 39.37, 'Meters to Inches'),
(1, 3, 3.28 , 'Meters to Foot'),

(2, 1, 0.02, 'Inches to Meters'),
(2, 3, 0.08, 'Inches to Foot'),

(3, 1, 0.03, 'Foot to Meters'),
(3, 2, 12  , 'Foot to Inches');