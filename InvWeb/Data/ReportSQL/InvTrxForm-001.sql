declare @trxTypeId int = 1;
declare @trxStatus int =1;

SELECT trx.Id, trx.InvStoreId, trx.DtTrx, trx.UserId, trx.Remarks
,stype.Type ,  stat.Status, store.StoreName
FROM dbo.InvTrxHdrs trx
left join dbo.InvTrxHdrStatus stat on stat.Id = trx.InvTrxHdrStatusId
left join dbo.InvTrxTypes stype on stype.Id = trx.InvTrxTypeId
left join dbo.InvStores store on store.Id = trx.InvStoreId
where trx.InvTrxTypeId = @trxTypeId and trx.InvTrxHdrStatusId = @trxStatus;


SELECT
hdr.Id
,dtl.Id
,dtl.BatchNo
,dtl.LotNo
,item.Description
,dtl.ItemQty
,uom.uom
FROM dbo.InvTrxDtls dtl
join dbo.InvTrxHdrs hdr on hdr.Id = dtl.InvTrxHdrId
left join dbo.InvItems item on item.Id = dtl.InvItemId
left join dbo.InvUoms uom on uom.Id = dtl.InvUomId

where hdr.InvTrxTypeId = @trxTypeId and hdr.InvTrxHdrStatusId = @trxStatus;
;



select stypes.Id, stypes.Type from dbo.InvTrxTypes stypes
