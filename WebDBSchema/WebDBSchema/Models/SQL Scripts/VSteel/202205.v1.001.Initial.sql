-- Stores --

IF NOT EXISTS (SELECT 1 FROM InvStores)
BEGIN
	insert into InvStores("StoreName") values 
	('Davao Main');
END

--Unit of Measure--
IF NOT EXISTS (SELECT 1 FROM InvUoms)
BEGIN
	insert into InvUoms([uom]) values
	('Meters'),('Inches'),('Foot');
END

-- Trx Types -- Receive, release, adjust.
IF NOT EXISTS (SELECT 1 FROM InvTrxTypes)
BEGIN
	insert into InvTrxTypes([Type]) values
	('Receive'),('Release'),('Adjust');
END

-- Trx Status 

IF NOT EXISTS (SELECT 1 FROM InvTrxHdrStatus)
BEGIN
	insert into InvTrxHdrStatus([Status],[OrderNo]) values
	('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);
END

-- PO Status 
IF NOT EXISTS (SELECT 1 FROM InvUoms)
BEGIN
	insert into InvUoms([uom]) values
	('METER'), ('FEET'), ('PC'), ('BOX');
END

--Transaction Operators
IF NOT EXISTS (SELECT 1 FROM InvTrxDtlOperators)
BEGIN
	insert into InvTrxDtlOperators([Description],[Operator]) values 
	('Add', '+'),('Subtract', '-');
END

--Inventory Warning Types
IF NOT EXISTS (SELECT 1 FROM InvWarningTypes)
BEGIN
	insert into InvWarningTypes([Desc]) values
	('Reorder'),('Warning'),('Critical');
END 


--Inventory Category
IF NOT EXISTS (SELECT 1 FROM InvCategories)
BEGIN
	insert into InvCategories([Code],[Description],[Remarks]) values
	('001','Materials',null),
	('002','Equipments',null),
	('003','Others',null);
END 


IF NOT EXISTS (SELECT 1 FROM InvClassifications)
BEGIN
	-- Classifications --
	insert into InvClassifications([Classification]) values
	('Others');
END


IF NOT EXISTS (SELECT 1 FROM InvCustomSpecTypes)
BEGIN
	-- Classifications --
	insert into InvCustomSpecTypes([Type]) values
	('Numeric'),
	('Text');
END

IF NOT EXISTS (SELECT 1 FROM InvItemOrigins)
BEGIN
	insert into InvItemOrigins([Name],[Code]) values
	('JAPAN','JP'),
	('CHINA','CN'),
	('VIETNAM','VN'),
	('INDONESIA','IND'),
	('MALAYSIA','MLY'),
	('PHILIPPINES','PH'),
	('EUROPE','UK'),
	('SINGAPORE','SG'),
	('KOREA','KR'),
	('TAIWAN','TW'),
	('INDIA','IND'),
	('THAILAND','TH');
END



IF NOT EXISTS (SELECT 1 FROM InvItemBrands)
BEGIN
	insert into InvItemBrands([Name],[Code]) values
	('Energy Steel','ENC'),
	('Threeway Steel','TSC'),
	('VSTEEL','VSP'),
	('KASUGAI','KSJ'),
	('MEISHIE','MS'),
	('KITZ','KZ'),
	('BENKAN','BK');
END

