declare @itemdesc varchar(max)='';
declare @brand varchar(max)='';
declare @origin varchar(max)='';

--declare @maincat varchar(max)=''; --'FITTING, PLATE';
--declare @subcat varchar(max)='';
--declare @grade varchar(max)='';
--declare @material varchar(max)='';

select top 100000
invitem.Id 
--,trxhdrs.Id

,invitem.Code
,CASE WHEN (trxdtlop.Operator='+' AND trxstat.Status='Approved') then trxdtls.ItemQty else 0 end as QtyAdded
,CASE WHEN (trxdtlop.Operator='+' AND trxstat.Status='Request') then trxdtls.ItemQty else 0 end as QtyIncoming
,CASE WHEN (trxdtlop.Operator='-' AND trxstat.Status='Approved') then trxdtls.ItemQty else 0 end as QtyReleased
,CASE WHEN (trxdtlop.Operator='-' AND trxstat.Status='Request') then trxdtls.ItemQty else 0 end as QtyOutgoing
,uom.uom
,trxtype.Type
,trxstat.Status
,item.BatchNo
,item.LotNo
--,trxdtlop.Operator

,invitem.Description
--,maincat.Name as 'Main Category'
--,subcat.Name as 'Sub Category'

,brand.Name as 'Brand'
--,grade.Name as 'Steel Grade'
--,material.Name as 'Steel Material'
,origin.Name as 'Origin'
--,size.Name as 'Steel Size'

,store.StoreName
,area.Name as 'Location'
--,charindex(@itemdesc,invitem.Description) as 'charIndex'

from dbo.InvItems invitem

left join dbo.InvTrxDtls trxdtls on trxdtls.InvItemId = invitem.Id
left join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId
left join dbo.InvUoms uom on uom.Id = trxdtls.InvUomId

left join dbo.InvTrxHdrs trxhdrs on trxhdrs.Id = trxdtls.InvTrxHdrId
left join dbo.InvTrxHdrStatus trxstat on trxstat.Id = trxhdrs.InvTrxHdrStatusId
left join dbo.InvTrxTypes trxtype on trxtype.Id = trxhdrs.InvTrxTypeId

left join dbo.InvStores store on store.Id = trxhdrs.InvStoreId

left join dbo.InvTrxDtlxItemMasters dtlxitem on dtlxitem.InvTrxDtlId = trxdtls.Id
left join dbo.InvItemMasters item on item.Id = dtlxitem.InvItemMasterId
left join dbo.InvItemBrands brand on brand.Id = item.InvItemBrandId
left join dbo.InvItemOrigins origin on origin.Id = item.InvItemOriginId
left join dbo.InvStoreAreas area on area.Id = item.InvStoreAreaId


where trxstat.Status not in ('Cancelled')
AND ( @itemdesc is null or RTRIM(@itemdesc)='' or charindex(@itemdesc, invitem.Description)>0 )
AND ( @origin is null or RTRIM(@origin)='' or charindex(@origin, origin.Name)>0 )
AND ( @brand is null or RTRIM(@brand)='' or charindex(@brand, brand.Name)>0 )

--AND ( @subcat is null or RTRIM(@subcat)='' or charindex(subcat.Name, @subcat)>0 )
--AND ( @grade is null or RTRIM(@grade)='' or charindex(grade.Name, @grade)>0 )
--AND ( @material is null or RTRIM(@material)='' or charindex(material.Name, @material)>0 )

order by invitem.Id
;


