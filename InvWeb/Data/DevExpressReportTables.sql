
CREATE TABLE "Reports" (
    "Id" INTEGER NOT NULL IDENTITY PRIMARY KEY,
    "Name" varchar(250) NULL,
    "DisplayName" varchar(250) NULL,
    "LayoutData" varbinary(max) NULL
);

CREATE TABLE "SqlDataConnections" (
    "Id" INTEGER NOT NULL IDENTITY PRIMARY KEY ,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "ConnectionString" nvarchar(max) NULL
);

CREATE TABLE "JsonDataConnections" (
    "Id" INTEGER NOT NULL IDENTITY PRIMARY KEY ,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "ConnectionString" nvarchar(max) NULL
);
