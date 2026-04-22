CREATE SCHEMA [identity];
GO

CREATE SCHEMA shared;
GO

CREATE SCHEMA inv;
GO

CREATE SCHEMA rep;
GO

CREATE SCHEMA crm;
GO


CREATE TABLE [identity].tenant
(
	id                  INT PRIMARY KEY IDENTITY(1,1),
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
	id                      INT PRIMARY KEY IDENTITY(1,1),
	tenant_id               INT NOT NULL REFERENCES [identity].tenant(id),
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

	created_by              INT NOT NULL REFERENCES [identity].[user](id),
	created_at              DATETIME NOT NULL,
	updated_by              INT NULL REFERENCES [identity].[user](id),
	updated_at              DATETIME NULL,
	deleted_by              INT NULL REFERENCES [identity].[user](id),
	deleted_at              DATETIME NULL,

	FOREIGN KEY (tenant_id) REFERENCES [identity].tenant (id),

	UNIQUE(tenant_id, normalized_user_name)
);

ALTER TABLE [identity].tenant
ADD FOREIGN KEY (created_by) REFERENCES [identity].[user](id),
FOREIGN KEY (updated_by) REFERENCES [identity].[user](id),
FOREIGN KEY (deleted_by) REFERENCES [identity].[user](id);

CREATE TABLE inv.screen
(
	id					INT PRIMARY KEY IDENTITY,
	uuid				UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	tenant_id			INT NOT NULL REFERENCES [identity].tenant(id),
	[name]				NVARCHAR(100) NOT NULL,
	normalized_name		NVARCHAR(100) NOT NULL,
	screen_code			NVARCHAR(40) NOT NULL,
	[description]		NVARCHAR(500) NOT NULL,
	default_resolution	NVARCHAR(50) NOT NULL,
	orientation			NVARCHAR(50) NOT NULL,
	[location]			NVARCHAR(100) NOT NULL,
	address_line		NVARCHAR(200) NULL,
	tag					NVARCHAR(50) NOT NULL,
	country_code		NVARCHAR(50) NOT NULL,
	city				NVARCHAR(50) NOT NULL,
	timezone			NVARCHAR(50) NOT NULL,
	is_active			BIT NOT NULL DEFAULT 1,

	created_by          INT NOT NULL REFERENCES [identity].[user](id),
	created_at          DATETIME NOT NULL,
	updated_by          INT NULL REFERENCES [identity].[user](id),
	updated_at          DATETIME NULL,
	deleted_by          INT NULL REFERENCES [identity].[user](id),
	deleted_at          DATETIME NULL
);

CREATE TABLE inv.screen_supported_media
(
	id				INT	PRIMARY KEY IDENTITY(1,1),
	screen_id		INT NOT NULL REFERENCES inv.screen(id),
	media_type		NVARCHAR(50) NOT NULL,
	is_deleted		BIT NOT NULL DEFAULT 0,

	created_by		INT NOT NULL REFERENCES [identity].[user](id),
	created_at		DATETIME2 NOT NULL,
	deleted_by		INT NULL REFERENCES [identity].[user](id),
	deleted_at		DATETIME2 NULL
);

CREATE TABLE inv.screen_operating_hour
(
	id						INT PRIMARY KEY IDENTITY(1,1),
	screen_id				INT NOT NULL REFERENCES inv.screen(id),
	day_of_week				NVARCHAR(50) NOT NULL,
	open_time				TIME NOT NULL,
	close_time				TIME NOT NULL,
	audience_source			NVARCHAR(50) NOT NULL,
	estimated_impression	INT NULL,
	is_deleted				BIT NOT NULL DEFAULT 0,

	created_by				INT NOT NULL REFERENCES [identity].[user](id),
	created_at				DATETIME2 NOT NULL,
	deleted_by				INT NULL REFERENCES [identity].[user](id),
	deleted_at				DATETIME2 NULL
);

CREATE TABLE crm.advertiser
(
	id				INT PRIMARY KEY IDENTITY(1,1),
	uuid			UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	tenant_id		INT NOT NULL REFERENCES [identity].tenant(id),
	[name]			NVARCHAR(100) NOT NULL,
	contact_name	NVARCHAR(60) NOT NULL,
	contact_email	NVARCHAR(60) NOT NULL,
	contact_phone	NVARCHAR(20) NOT NULL,
	is_active		BIT NOT NULL DEFAULT 1,
	is_deleted		BIT NOT NULL DEFAULT 0,

	created_by		INT NOT NULL REFERENCES [identity].[user](id),
	created_at		DATETIME2 NOT NULL,
	updated_by		INT  NULL REFERENCES [identity].[user](id),
	updated_at		DATETIME2 NULL,
	deleted_by		INT NULL REFERENCES [identity].[user](id),
	deleted_at		DATETIME2 NULL
);

CREATE TABLE campaign
(
	id				INT PRIMARY KEY IDENTITY(1,1),
	uuid			UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	tenant_id		INT NOT NULL REFERENCES [identity].tenant(id),
	advertiser_id	INT NOT NULL REFERENCES crm.advertiser(id),
	[name]			NVARCHAR(100) NOT NULL,
	campaign_code	NVARCHAR(40) NOT NULL,
	[start_date]	DATE	NOT NULL,
	end_date	DATE NOT NULL,
	[status]		NVARCHAR(50) NOT NULL,
	note			NVARCHAR(255) NULL,
	is_deleted		BIT DEFAULT 0,

	created_by		INT NOT NULL REFERENCES [identity].[user](id),
	created_at		DATETIME2 NOT NULL,
	updated_by		INT  NULL REFERENCES [identity].[user](id),
	updated_at		DATETIME2 NULL,
	deleted_by		INT NULL REFERENCES [identity].[user](id),
	deleted_at		DATETIME2 NULL
);

CREATE TABLE campaign_schedule
(
	id					INT PRIMARY KEY IDENTITY(1,1),
	campaign_id			INT NOT NULL REFERENCES campaign(id),
	start_date_time		DATETIME2 NOT NULL,
	end_date_time		DATETIME2 NOT NULL,
	is_deleted			BIT NOT NULL DEFAULT 0,

	created_by			INT			NOT NULL REFERENCES [identity].[user](id),
    created_at			DATETIME2	NOT NULL,
    deleted_by			INT    NULL REFERENCES [identity].[user](id),
    deleted_at			DATETIME2           NULL
);

CREATE TABLE media_library
(	
	id				INT PRIMARY KEY IDENTITY(1,1),
	uuid			UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	tenant_id		INT NOT NULL REFERENCES [identity].tenant(id),
	display_name	NVARCHAR(100) NOT NULL,
	[file_name]		NVARCHAR(255) NULL,
	file_url		NVARCHAR(500) NULL,
	resolution		NVARCHAR(50)  NULL,
	[status]		NVARCHAR(50) NOT NULL,
	duration_sec	INT NULL,
	is_video		BIT NULL,
	is_deleted		BIT NOT NULL DEFAULT 0,
	
	uploaded_by		INT NULL REFERENCES [identity].[user](id),
	uploaded_at		DATETIME2 NULL,
	created_by		INT NOT NULL REFERENCES [identity].[user](id),
	created_at		DATETIME2 NOT NULL,
	deleted_by		INT NULL REFERENCES [identity].[user](id),
	deleted_at		DATETIME2 NULL
);

CREATE TABLE campaign_screen_media
(
	id				INT PRIMARY KEY IDENTITY(1,1),
	campaign_id		INT NOT NULL,
	media_id		INT NOT NULL,
	screen_id		INT NOT NULL,
	play_sequence	INT NOT NULL
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

CREATE TABLE shared.tag
(
	id			INT PRIMARY KEY IDENTITY(1,1),
	tenant_id	INT NOT NULL REFERENCES [identity].tenant(id),
	[name]		NVARCHAR(100) NOT NULL,
	is_active	BIT NOT NULL DEFAULT 1,

	created_by			INT	NOT NULL REFERENCES [identity].[user](id),
    created_at			DATETIME2	NOT NULL,
    updated_by			INT NULL REFERENCES [identity].[user](id),
    updated_at			DATETIME2	NULL
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
