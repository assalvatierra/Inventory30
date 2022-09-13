-- Stores --

IF NOT EXISTS (SELECT 1 FROM InvStores)
BEGIN
	insert into InvStores("StoreName") values 
	('Davao Main');
END
