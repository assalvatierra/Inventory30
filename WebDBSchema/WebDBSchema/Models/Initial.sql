-- Stores --
insert into InvStores("StoreName") values 
('Cebu Branch 1'),('Davao Main'),('Gensan Branch 1');

--Unit of Measure--
insert into InvUoms([uom]) values
('pc'),('box'),('dozen');

-- Items --
insert into InvItems([Description],[Remarks],[InvUomId]) values
('Milk', 'Fresh Milks', 2),
('Eggs', 'Fresh Eggs', 3);

-- Suppliers --
insert into InvSuppliers([Name],[Remarks]) values
('Dairy Farm','');

-- Classifications --
insert into InvClassifications([Classification]) values
('Food/Perishables');


-- Trx Types -- Receive, release, adjust.
insert into InvTrxTypes([Type]) values
('Receive'),('Release'),('Adjust');


-- Trx Status 
insert into InvTrxHdrStatus([Status],[OrderNo]) values
('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);


-- PO Status 
insert into InvPoHdrStatus([Status],[OrderNo]) values
('Request',1), ('Approved',2), ('Closed',3), ('Cancelled',4);