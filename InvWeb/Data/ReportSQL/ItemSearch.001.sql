declare @itemdesc varchar(max)='';
declare @maincat varchar(max)=''; --'FITTING, PLATE';
declare @subcat varchar(max)='';
declare @brand varchar(max)='';
declare @grade varchar(max)='';
declare @material varchar(max)='';
declare @origin varchar(max)='';

select top 1000
invitem.Id 
,invitem.Code
,' ' as '*'
,trxhdrs.Id

,CASE WHEN (trxdtlop.Operator='+' AND trxstat.Status='Approved') then trxdtls.ItemQty else 0 end as QtyAdded
,CASE WHEN (trxdtlop.Operator='+' AND trxstat.Status='Request') then trxdtls.ItemQty else 0 end as QtyIncoming
,CASE WHEN (trxdtlop.Operator='-' AND trxstat.Status='Approved') then trxdtls.ItemQty else 0 end as QtyReleased
,CASE WHEN (trxdtlop.Operator='-' AND trxstat.Status='Request') then trxdtls.ItemQty else 0 end as QtyOutgoing
,uom.uom
,trxtype.Type
,trxstat.Status
,trxdtlop.Operator

,invitem.Description
,maincat.Name as 'Main Category'
,subcat.Name as 'Sub Category'
,brand.Name as 'Steel Brand'
,grade.Name as 'Steel Grade'
,material.Name as 'Steel Material'
,origin.Name as 'Steel Origin'
,size.Name as 'Steel Size'

,store.StoreName

from dbo.InvItems invitem
left join dbo.InvItemSpec_Steel itemspec on itemspec.InvItemId = invitem.Id
left join dbo.SteelSizes size on size.Id = itemspec.SteelSizeId
left join dbo.SteelMainCats maincat on maincat.Id=itemspec.SteelMainCatId
left join dbo.SteelSubCats subcat on subcat.Id=itemspec.SteelSubCatId
left join dbo.SteelBrands brand on brand.Id = itemspec.SteelBrandId
left join dbo.SteelMaterialGrades grade on grade.Id = itemspec.SteelMaterialGradeId
left join dbo.SteelMaterials material on material.Id=itemspec.SteelMaterialId
left join dbo.SteelOrigins origin on origin.Id=itemspec.SteelOriginId

left join dbo.InvTrxDtls trxdtls on trxdtls.InvItemId = invitem.Id
left join dbo.InvTrxDtlOperators trxdtlop on trxdtlop.Id = trxdtls.InvTrxDtlOperatorId
left join dbo.InvUoms uom on uom.Id = trxdtls.InvUomId

left join dbo.InvTrxHdrs trxhdrs on trxhdrs.Id = trxdtls.InvTrxHdrId
left join dbo.InvTrxHdrStatus trxstat on trxstat.Id = trxhdrs.InvTrxHdrStatusId
left join dbo.InvTrxTypes trxtype on trxtype.Id = trxhdrs.InvTrxTypeId
--left join dbo.InvTrxApprovals trxapp on trxapp.InvTrxHdrId = trxhdrs.Id

left join dbo.InvStores store on store.Id = trxhdrs.InvStoreId

where 
( @maincat is null or RTRIM(@maincat)='' or charindex(maincat.Name, @maincat)>0 )
AND ( @subcat is null or RTRIM(@subcat)='' or charindex(subcat.Name, @subcat)>0 )
AND ( @brand is null or RTRIM(@brand)='' or charindex(brand.Name, @brand)>0 )
AND ( @grade is null or RTRIM(@grade)='' or charindex(grade.Name, @grade)>0 )
AND ( @material is null or RTRIM(@material)='' or charindex(material.Name, @material)>0 )
AND ( @origin is null or RTRIM(@origin)='' or charindex(origin.Name, @origin)>0 )

order by invitem.Id
;


