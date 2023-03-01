insert into InvStores("StoreName") values ('Cebu Branch 1'),('Davao Main'),('Gensan Branch 1');

select * from InvStores;

-- Get Inventory Count grouped per Item and Store
select asg.InvItemId, asg.InvStoreId, UomId = MIN(asg.InvUomId), ItemQty = SUM(asg.ItemQty)  from (
	select dtls.*, hdr.InvStoreId from InvTrxDtls dtls
	left join InvTrxHdrs hdr
	on dtls.InvTrxHdrId = hdr.Id
	) as asg
Group by  asg.InvStoreId, asg.InvItemId
