

-- Store Receiving --
-- Using Store 2
insert  into InvTrxHdrs([InvStoreId], [DtTrx], [UserId], [Remarks], [InvTrxTypeId], [InvTrxHdrStatusId]) values
(2, '09/27/2021', 'admin@gmail.com', '', 1, 2),
(2, '09/29/2021', 'admin@gmail.com', '', 1, 1)
;

insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values
(1, 1, 10, 1, 1),
(1, 1, 5, 2, 1),
(1, 1, 3, 3, 1),

(2, 1, 5, 1, 1),
(2, 1, 5, 2, 1),
(2, 1, 5, 3, 1)
;

-- Store Releasing --
-- Using Store 2
insert  into InvTrxHdrs([InvStoreId], [DtTrx], [UserId], [Remarks], [InvTrxTypeId], [InvTrxHdrStatusId]) values
(2, '09/28/2021', 'admin@gmail.com', '', 2, 2),
(2, '10/18/2021', 'admin@gmail.com', '', 2, 1)
;

insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values
(3, 1, 3, 1, 2),
(3, 1, 2, 2, 2),
(3, 1, 1, 3, 2),

(4, 1, 3, 1, 2),
(4, 1, 2, 3, 2)
;

-- Purchase Request --
-- Using Store 2
insert  into InvPoHdrs([InvSupplierId],[InvStoreId],[DtPo],[UserId],[InvPoHdrStatusId]) values
(3, 2, '09/26/2021', 'admin@gmail.com', 1),
(2, 2, '09/26/2021', 'admin@gmail.com', 1)
;

insert into InvPoItems([InvPoHdrId],[InvItemId],[ItemQty],[InvUomId]) values
(1, 1, 10, 1),
(1, 2, 5, 1),
(1, 3, 5, 1),
(1, 4, 5, 1),

(2, 2, 3, 3),
(2, 3, 3, 3)
;



-- Store Adjustment --
-- Using Store 2
insert  into InvTrxHdrs([InvStoreId], [DtTrx], [UserId], [Remarks], [InvTrxTypeId], [InvTrxHdrStatusId]) values
(2, '09/30/2021', 'admin@gmail.com', '', 3, 1)
;

insert into InvTrxDtls([InvTrxHdrId],[InvUomId],[ItemQty],[InvItemId],[InvTrxDtlOperatorId]) values
(4, 1, 5, 1, 2)
;
