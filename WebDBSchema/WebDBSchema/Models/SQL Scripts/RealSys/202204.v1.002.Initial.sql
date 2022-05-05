-- Stores --

IF NOT EXISTS (SELECT 1 FROM InvStores)
BEGIN
	insert into InvStores("StoreName") values 
	('Cebu Branch 1'),('Davao Main'),('Gensan Branch 1');
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