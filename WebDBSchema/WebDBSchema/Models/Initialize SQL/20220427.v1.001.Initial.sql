﻿-- Stores --
insert into InvStores("StoreName") values 
('Cebu Branch 1'),('Davao Main'),('Gensan Branch 1');

--Unit of Measure--
insert into InvUoms([uom]) values
('Meters'),('Inches'),('Foot');

-- Trx Types -- Receive, release, adjust.
insert into InvTrxTypes([Type]) values
('Receive'),('Release'),('Adjust');

-- Trx Status 
insert into InvTrxHdrStatus([Status],[OrderNo]) values
('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);

-- PO Status 
insert into InvPoHdrStatus([Status],[OrderNo]) values
('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);

--Transaction Operators
insert into InvTrxDtlOperators([Description],[Operator]) values 
('Add', '+'),('Subtract', '-');

--Inventory Warning Types
insert into InvWarningTypes([Desc]) values
('Reorder'),('Warning'),('Critical');