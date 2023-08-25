-- User Roles --
insert into AspNetRoles([Name],[NormalizedName],[ConcurrencyStamp]) values 
(1,'Admin', null, null),(2,'Store', null, null),(3,'Purchaser', null, null);

-- User Roles w/ init ADMIN on existing table--
insert into AspNetRoles([Id],[Name],[NormalizedName],[ConcurrencyStamp]) values 
(2,'Store', null, null),(3,'Purchaser', null, null);
