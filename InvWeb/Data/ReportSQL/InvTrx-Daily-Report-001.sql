select 
---- Trx Hdr ----
store.Id as 'StoreId'
,store.StoreName
,hdr.Id as 'HdrId'
,hdr.DtTrx as 'TrxDate'
,hdr.Remarks
,approval.ApprovedBy
,approval.ApprovedDate
,approval.VerifiedBy
,approval.VerifiedDate
,trxStat.Status as 'TrxStatus'


from dbo.InvTrxHdrs hdr 
left join dbo.InvStores store on store.Id = hdr.InvStoreId
left join dbo.InvTrxApprovals approval on approval.InvTrxHdrId = hdr.Id
left join dbo.InvTrxHdrStatus trxStat on trxStat.Id = hdr.InvTrxHdrStatusId
;

select 
---- Trx Hdr ----
store.Id as 'StoreId'
,store.StoreName
,hdr.Id as 'HdrId'
,hdr.DtTrx as 'TrxDate'
,hdr.Remarks
,approval.ApprovedBy
,approval.ApprovedDate
,approval.VerifiedBy
,approval.VerifiedDate
,trxStat.Status as 'TrxStatus'

---- Trx Dtl ----
,dtl.Id
,dtl.ItemQty
,uom.uom
,dtl.BatchNo
,dtl.LotNo
,item.Code
,item.Description
,sBrand.Name as 'Steel Brand'
,sCat.Name as 'Steel  Category'
,sSubCat.Name as 'Steel SubCategory'
,sMat.Name as 'Steel Material'
,sGrade.Name as 'Material Grade'
,sOrigin.Name as 'Steel Origin'

from dbo.InvTrxDtls dtl
join dbo.InvTrxHdrs hdr on hdr.Id = dtl.InvTrxHdrId
left join dbo.InvStores store on store.Id = hdr.InvStoreId
left join dbo.InvTrxApprovals approval on approval.InvTrxHdrId = hdr.Id
left join dbo.InvTrxHdrStatus trxStat on trxStat.Id = hdr.InvTrxHdrStatusId

left join dbo.InvUoms uom on uom.Id = dtl.InvUomId
left join dbo.InvItems item on item.Id = dtl.InvItemId
left join dbo.InvItemSpec_Steel spec on spec.InvItemId = item.Id
left join dbo.SteelBrands sBrand on sBrand.Id = spec.SteelBrandId
left join dbo.SteelMainCats sCat on sCat.Id = spec.SteelMainCatId
left join dbo.SteelSubCats sSubCat on sSubCat.Id = spec.SteelSubCatId
left join dbo.SteelMaterials sMat on sMat.Id = spec.SteelMaterialId
left join dbo.SteelMaterialGrades sGrade on sGrade.Id = spec.SteelMaterialGradeId
left join dbo.SteelOrigins sOrigin on sOrigin.Id = spec.SteelOriginId
;

