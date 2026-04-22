CREATE SCHEMA [identity];
GO

CREATE SCHEMA shared;
GO

CREATE SCHEMA asset;
GO

CREATE SCHEMA cms;
GO

CREATE SCHEMA inv;
GO

CREATE SCHEMA campaign;
GO

CREATE SCHEMA rep;
GO

CREATE SCHEMA crm;
GO


CREATE TABLE [identity].tenant
(
	id                  INT PRIMARY KEY IDENTITY(1,1),
    tenant_uuid         UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    tenant_code         NVARCHAR(40) NOT NULL,
	[name]              NVARCHAR(100) NOT NULL,
	normalized_name     NVARCHAR(100) NOT NULL UNIQUE,
	default_currency    NVARCHAR(10) NOT NULL,
    contact_email       NVARCHAR(100) NOT NULL,
    contact_phone       NVARCHAR(20) NOT NULL,
    contact_name        NVARCHAR(50) NOT NULL,
    time_zone           NVARCHAR(50) NOT NULL,
    is_active           BIT NOT NULL DEFAULT 1,
	is_deleted          BIT NOT NULL DEFAULT 0,

	created_by          INT NOT NULL,
	created_at          DATETIME NOT NULL,
	updated_by			INT NULL,
	updated_at			DATETIME NULL,
	deleted_by          INT NULL,
	deleted_at          DATETIME NULL,

	UNIQUE (id, normalized_name)
);

CREATE TABLE [identity].[user]
(
	id                      UNIQUEIDENTIFIER PRIMARY KEY,
	tenant_id               INT NOT NULL,
	[user_name]             NVARCHAR(100) NOT NULL,
	normalized_user_name    NVARCHAR(100) NOT NULL,
	[name]                  NVARCHAR(100) NOT NULL,
	[sur_name]              NVARCHAR(50) NOT NULL,
	email                   NVARCHAR(100) NOT NULL,
	normalized_email        NVARCHAR(100) NOT NULL,
	password_hash           NVARCHAR(150) NOT NULL,
	phone_number            NVARCHAR(20) NULL,
	user_role               NVARCHAR(50) NOT NULL,
	is_active               BIT NOT NULL,
	is_deleted              BIT NOT NULL DEFAULT 0,

	created_by              UNIQUEIDENTIFIER NOT NULL,
	created_at              DATETIME NOT NULL,
	updated_by              UNIQUEIDENTIFIER NULL,
	updated_at              DATETIME NULL,
	deleted_by              UNIQUEIDENTIFIER NULL,
	deleted_at              DATETIME NULL,

	FOREIGN KEY (tenant_id) REFERENCES [identity].tenant (id),

	UNIQUE(tenant_id, normalized_user_name)
);

CREATE TABLE shared.audit_log
(
	id BIGINT IDENTITY,
	entity_type NVARCHAR(50) NOT NULL,
	[entity_id] UNIQUEIDENTIFIER NOT NULL,
	[action] NVARCHAR(20) NOT NULL,
	changed_by UNIQUEIDENTIFIER NOT NULL,
	changed_at DATETIME NOT NULL DEFAULT GETUTCDATE(),
	old_value NVARCHAR(MAX) NULL,
	new_value NVARCHAR(MAX) NULL
);

CREATE TABLE shared.list_item_category
(
	id INT PRIMARY KEY IDENTITY(1,1),
    tenant_id UNIQUEIDENTIFIER NULL,
	category NVARCHAR(100) NOT NULL,
	category_code NVARCHAR(50) NOT NULL
);

CREATE TABLE shared.list_item
(
	id INT PRIMARY KEY IDENTITY(1,1),
    tenant_id UNIQUEIDENTIFIER NULL,
	category_id INT NOT NULL,
	item NVARCHAR(100) NOT NULL,
	code NVARCHAR(50) NOT NULL,
	is_active BIT NOT NULL DEFAULT 1,
	
	FOREIGN KEY (category_id) REFERENCES shared.list_item_category(id)
);
