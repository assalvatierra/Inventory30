select 
item.Id as 'MasterId'
,inv.Id as 'ItemId'
,inv.InvCategoryId as 'categoryId'
,inv.Description
,brand.Name as 'Brand'
,origin.Name as 'Origin'
,store.StoreName
,area.Name as 'Location'
,item.ItemQty


from dbo.InvItemMasters item
inner join dbo.InvItems inv on inv.Id = item.InvItemId
left join dbo.InvItemBrands brand on brand.Id = item.InvItemBrandId
left join dbo.InvItemOrigins origin on origin.Id = item.InvItemOriginId
left join dbo.InvStoreAreas area on area.Id = item.InvStoreAreaId
left join dbo.InvStores store on store.Id = area.InvStoreId


order by inv.Id
;

select dtlxitem.InvItemMasterId as 'MasterItemId', trxdtls.ItemQty,trxdtlop.Operator, trxStatus.Status, trxtype.Type from dbo.InvTrxDtls trxdtls 
inner join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id
inner join dbo.InvTrxHdrs trxHdr on trxHdr.Id = trxdtls.InvTrxHdrId
inner join dbo.InvTrxHdrStatus trxStatus on trxStatus.Id = trxHdr.InvTrxHdrStatusId
inner join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId
inner join dbo.InvTrxTypes trxtype on trxtype.Id = trxHdr.InvTrxTypeId



