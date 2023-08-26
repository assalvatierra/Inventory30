DECLARE @Today AS DATETIME
SET @Today = GetDate()

--Sample Items --
IF NOT EXISTS (SELECT 1 FROM InvItems)
BEGIN
	insert into InvItems([ItemCode],[Description],[Remarks],[InvUomId],[InvCategoryId]) values
	('001','Steel', '', 1, 1),
	('002','Pipe' , '', 1, 1),
	('003','Plate', '', 1, 1)
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
	(1, 2,'',50, @Today ,'30 Days','admin@gmail.com');
END

-- Store 1 : Transaction Headers --
insert into InvTrxHdrs([InvStoreId],[DtTrx],[UserId],[Remarks],[InvTrxTypeId],[InvTrxHdrStatusId]) values
(1,@Today,'admin@gmail.com','',1,1);

--Store 1: Transaction Detalis --
insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values 
(1, 1, 1, 2, 1);

-- Store 2 : Transaction Headers --
insert into InvTrxHdrs([InvStoreId],[DtTrx],[UserId],[Remarks],[InvTrxTypeId],[InvTrxHdrStatusId]) values
(1,@Today,'admin@gmail.com','',1,1);

--Store 2: Transaction Detalis --
insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values
(1, 1, 1, 2, 1);

--Inv Classifications --
insert into InvItemClasses ([InvItemId],[InvClassificationId]) values
(2, 1);

--Inv Items Warning Levels --
insert into InvWarningLevels([InvItemId],[Level1],[Level2],[InvWarningTypeId],[InvUomId]) values
(2, 11, 15, 1, 1);


--Inv Uom Conversions
insert into InvUomConversions([InvUomId_base],[InvUomId_into],[Factor],[Description]) values
(1, 2, 39.37, 'Meters to Inches'),
(1, 3, 3.28 , 'Meters to Foot'),

(2, 1, 0.02, 'Inches to Meters'),
(2, 3, 0.08, 'Inches to Foot'),

(3, 1, 0.03, 'Foot to Meters'),
(3, 2, 12  , 'Foot to Inches');


-- Inv Custom Specs

IF NOT EXISTS (SELECT 1 FROM InvCustomSpecs)
BEGIN
	-- Custom Specs --
	insert into InvCustomSpecs([SpecName],[InvCustomSpecTypeId],[Order],[Measurement],[Remarks]) values
	('Weight', 1, 1, '',''),
	('Color' , 2, 1, '',''),
	('Conductivity', 1, 1, '',''),
	('Strength' , 1, 1, '',''),
	('Toughness', 1, 1, '',''),
	('Ductility', 1, 1, '','');
END

IF NOT EXISTS (SELECT 1 FROM InvItemSysDefinedSpecs)
BEGIN
	insert into InvItemSysDefinedSpecs([SpecName],[SpecCode],[SpecGroup]) values
	('Length', 'LGT', 'Size'),
	('Width' , 'WDT', 'Size');
END


-- Inv Steel Specs

IF NOT EXISTS (SELECT 1 FROM SteelBrands)
BEGIN
	insert into SteelBrands([Name],[Code]) values
	('Kasugai', 'KASUGAI'),
	('Energy Steel' , 'ENERGY STEEL')
END


IF NOT EXISTS (SELECT 1 FROM SteelMainCats)
BEGIN
	insert into SteelMainCats([Name],[Code]) values
	('Pipe', 'PIPE'),
	('Fittings' , 'FITTINGS'),
	('Flange', 'FLANGE'),
	('Plate', 'PLATE'),
	('Beams', 'BEAMS')
END




IF NOT EXISTS (SELECT 1 FROM SteelMaterialGrades)
BEGIN
	insert into SteelMaterialGrades([Name],[Code]) values
	('ASTM A53B / A106B', ''),
	('ASTM A234 WPB' , 'FITTINGS'),
	('ASTM A105', 'FLANGE'),
	('ASTM A36', 'PLATE'),
	('ASTM A516-70', 'BEAMS')
END



IF NOT EXISTS (SELECT 1 FROM SteelMaterials)
BEGIN
	insert into SteelMaterials([Name],[Code]) values
	('Stainless Steel', ''),
	('Carbon' , ''),
	('Alloy', '')
END


IF NOT EXISTS (SELECT 1 FROM SteelOrigins)
BEGIN
	insert into SteelOrigins([Name],[Code]) values
	('Philippines', 'PH'),
	('Japan' , 'JPM'),
	('China', 'CHN'),
	('Korea', 'KRN'),
	('India', 'IND')
END



IF NOT EXISTS (SELECT 1 FROM SteelSubCats)
BEGIN
	insert into SteelSubCats([Name],[Code]) values
	('CS SMLS PIPE', ''),
	('CS ELBOW', ''),
	('CS TEE', ''),
	('CS RED TEE', ''),
	('CS CON RED', ''),
	('S.O. FLANGE', ''),
	('BLIND FLANGE', ''),
	('MS PLATE', ''),
	('BOILER PLATE', ''),
	('WBEAMS', '')
END