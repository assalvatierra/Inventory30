SELECT * 
FROM
(
	select 
	item.Id as 'MasterId'
	,inv.Id as 'ItemId'
	--,item.ItemQty
	--,dataRel.Qty as 'ReleasedQty'
	--,dataHold.Qty as 'ItemOnHoldQty'
	,(item.ItemQty - coalesce(dataRel.Qty,0) )as 'StockOnHand' 
	--,(item.ItemQty - coalesce(dataRel.Qty,0) - coalesce(dataHold.Qty,0) ) as 'AvailableQty' 
	,itemcat.Description as 'Category'
	--,inv.Code
	--,inv.Description
	--,item.BatchNo
	--,item.LotNo
	--,brand.Name as 'Brand'
	--,origin.Name as 'Origin'
	,store.StoreName
	,area.Name as 'Location'

	from dbo.InvItemMasters item
	inner join dbo.InvItems inv on inv.Id = item.InvItemId
	left join dbo.InvCategories itemcat on itemcat.Id = inv.InvCategoryId
	--left join dbo.InvItemBrands brand on brand.Id = item.InvItemBrandId
	--left join dbo.InvItemOrigins origin on origin.Id = item.InvItemOriginId
	left join dbo.InvStoreAreas area on area.Id = item.InvStoreAreaId
	left join dbo.InvStores store on store.Id = area.InvStoreId
	left join (
		select dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'
		from dbo.InvTrxDtls trxdtls 
		inner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id
		inner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId
		inner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId
		inner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId
		inner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId
		where trxtype.Type = 'Release' 
		and trxStatus.Status = 'Closed'
		group by dtlxitem.InvItemMasterId
		) dataRel on dataRel.MasterItemId = item.Id
	left join (
		select dtlxitem.InvItemMasterId as 'MasterItemId', SUM(trxdtls.ItemQty) as 'Qty'
		from dbo.InvTrxDtls trxdtls 
		inner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id
		inner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId
		inner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId
		inner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId
		inner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId
		where trxtype.Type = 'Release' 
		and trxStatus.Status = 'Approve'
		group by dtlxitem.InvItemMasterId
		) dataHold on dataRel.MasterItemId = item.Id

) Item 
where item.StockOnHand > 0
--AND ( @itemdesc is null or RTRIM(@itemdesc)='' or charindex(@itemdesc, item.Description)>0 )
--AND ( @origin is null or RTRIM(@origin)='' or charindex(@origin, item.Origin)>0 )
--AND ( @brand is null or RTRIM(@brand)='' or charindex(@brand, item.Brand)>0 )
--AND ( @store is null or RTRIM(@store)='' or charindex(@store, item.StoreName)>0 )


order by Item.ItemId

;