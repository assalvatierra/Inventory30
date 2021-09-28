insert into InvStores("StoreName") values ('Cebu Branch 1'),('Davao Main'),('Gensan Branch 1');

select * from InvStores;

select * from InvTrxHdrs
where InvStoreId = 2 AND InvTrxTypeId = 1
