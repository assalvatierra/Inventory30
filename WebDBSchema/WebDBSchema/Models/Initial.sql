-- Stores --
insert into InvStores("StoreName") values 
('Cebu Branch 1'),('Davao Main'),('Gensan Branch 1');

--Unit of Measure--
insert into InvUoms([uom]) values
('pc'),('box'),('dozen');

-- Items --
insert into InvItems([Description],[Remarks],[InvUomId]) values
('Acer Mouse', 'Optical Mouse', 1),
('Acer Keyboard', 'Regular Keyboard', 1),
('Acer Monitor 22inch', '22 inch 1080p 60hz Monitor', 1),
('Acer Speaker', 'Regular Speaker', 1),
('Acer CPU Tower 10th Gen I504500', 'Intel Core i5 10th Gen, 8GB Ram, 500GB SSD', 1);

-- Suppliers --
insert into InvSuppliers([Name],[Remarks]) values
('Data Blitz Computer Center','SM Davao - Ecoland'),
('Acer Concept Store','SM Davao - Ecoland'),
('Silicon Valley','SM Davao - Ecoland'),
('Thinking Tools Inc','SM Davao - Ecoland');

-- Classifications --
insert into InvClassifications([Classification]) values
('Computer Mouse'),
('Computer Keyboards'),
('Computer Monitor'),
('Computer Towers'),
('Computer Peripherals'),
('Others');


-- Trx Types -- Receive, release, adjust.
insert into InvTrxTypes([Type]) values
('Receive'),('Release'),('Adjust');


-- Trx Status 
insert into InvTrxHdrStatus([Status],[OrderNo]) values
('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);


-- PO Status 
insert into InvPoHdrStatus([Status],[OrderNo]) values
('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);


insert into InvTrxDtlOperators([Description],[Operator]) values 
('Add', '+'),('Subtract', '-');

