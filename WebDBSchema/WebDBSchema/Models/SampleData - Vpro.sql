
--Sample Items --
insert into InvItems([Code],[Description],[Remarks],[InvUomId]) values
('GS15','NAPPA GENIUNE', 'BEIGE', 1),
('GS15L','NAPPA GENIUNE', 'BEIGE', 1),
('GS01','NAPPA GENIUNE', 'BLACK', 1),
('GS03','NAPPA GENIUNE', 'BROWN', 1),
('GS18','NAPPA GENIUNE', 'GREY', 1),
('PS01','NAPPA GENIUNE', 'BLACK', 1),
('PS04','NAPPA GENIUNE', 'BROWN', 1),
('PS01','NAPPA GENIUNE', 'LIGHT GREY', 1),
('PS15','NAPPA GENIUNE', 'BLACK', 1),

('PS07','LOW NAPPA', 'BURGUNDY RED', 1),
('PS10','LOW NAPPA', 'BENTLY BROWN', 1),
('PS10H','LOW NAPPA', 'BLACK', 1),
('PS15H','LOW NAPPA', 'BEIGE', 1),
('PS27','LOW NAPPA', 'DARK BLUE', 1),

('VP14S2G','VPRO PATTERN', 'BEIGE with DOUBLE STITCH BEIGE', 1),
('VP02S2Gr','VPRO PATTERN', 'GREY with DOUBLE STITCH GREY', 1),
('VP03S2G','VPRO PATTERN', 'BROWN with DOUBLE STITCH GOLD', 1),
('VP07SG','VPRO PATTERN', 'BURGUNDY RED with DOUBLE STITCH GOLD', 1),
('VP10S2B','VPRO PATTERN', 'BENTLEY BROWN with DOUBLE STITCH BENTLY', 1),
('VP01S2R','VPRO PATTERN', 'BLACK with DOUBLE STITCH RED', 1),
('VP01S2B','VPRO PATTERN', 'BLACK with DOUBLE STITCH BLACK', 1)

;

--Sample Suppliers --
insert into InvSuppliers([Name],[Remarks]) values
('HOW CHAIR','Davao'),
('JUNSHANG','Davao');

-- Classifications --
insert into InvClassifications([Classification]) values
('Leather'),
('Others');


-- Add items to suppliers
insert into InvSupplierItems([InvSupplierId],[InvItemId],[Remarks],[Price],[LastUpdate],[LeadTime],[UserId]) values
(1,1,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,2,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,3,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,4,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,5,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,6,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,7,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,8,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,9,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,10,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,11,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,12,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,13,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,14,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(2,15,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(2,16,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(2,17,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(1,18,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(2,19,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(2,20,'',0,'10/30/2021','30 Days','admin@gmail.com'),
(2,21,'',0,'10/30/2021','30 Days','admin@gmail.com');

-- Store 1 : Transaction Headers --
insert into InvTrxHdrs([InvStoreId],[DtTrx],[UserId],[Remarks],[InvTrxTypeId],[InvTrxHdrStatusId]) values
(1,'11/15/2021','admin@gmail.com','',1,1),
(1,'11/18/2021','admin@gmail.com','',1,2),
(1,'11/20/2021','admin@gmail.com','',2,1);

--Store 1: Transaction Detalis --
insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values 
(1, 1, 10, 1, 1),
(1, 1, 5, 8, 1),
(2, 1, 3, 15, 1),
(2, 1, 3, 20, 1),
(3, 1, 3, 1, 2);

-- Store 2 : Transaction Headers --
insert into InvTrxHdrs([InvStoreId],[DtTrx],[UserId],[Remarks],[InvTrxTypeId],[InvTrxHdrStatusId]) values
(2,'11/12/2021','admin@gmail.com','',1,1),
(2,'11/19/2021','admin@gmail.com','',1,2);

--Store 2: Transaction Detalis --
insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values 
(3, 1, 10, 2, 1),
(3, 1, 5, 5, 1),
(4, 1, 3, 9, 2),
(4, 1, 3, 2, 2);