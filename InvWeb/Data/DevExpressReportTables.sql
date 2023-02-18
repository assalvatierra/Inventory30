
CREATE TABLE "Reports" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Reports" PRIMARY KEY,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "LayoutData" varbinary(max) NULL
);

CREATE TABLE "SqlDataConnections" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_SqlDataConnections" PRIMARY KEY ,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "ConnectionString" nvarchar(max) NULL
);

CREATE TABLE "JsonDataConnections" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_JsonDataConnections" PRIMARY KEY ,
    "Name" nvarchar(max) NULL,
    "DisplayName" nvarchar(max) NULL,
    "ConnectionString" nvarchar(max) NULL
);