
CREATE TABLE dbo.campaign
(
	id INT PRIMARY KEY IDENTITY(1,1),
	campaign_code NVARCHAR(50) NOT NULL,
	tenant_id INT NOT NULL REFERENCES [identity].tenant(id),
	advertiser_id INT NULL REFERENCES crm.advertiser(id),
	[name] NVARCHAR(150) NOT NULL,
	[status] NVARCHAR(50) NOT NULL DEFAULT('DRAFT'),
	[start_date] DATE NOT NULL,
	[end_date] DATE NOT NULL,
	duration_in_days AS DATEDIFF (DAY, [start_date], end_date) PERSISTED,
	remarks NVARCHAR(255) NULL,
	is_locked BIT NOT NULL DEFAULT(0),
	created_by INT NOT NULL REFERENCES [identity].[user](id),
	created_at DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
	modified_by INT NULL REFERENCES [identity].[user](id),
	modified_at DATETIME2 NULL,
	is_deleted BIT NOT NULL DEFAULT(0),
	deleted_by INT NULL REFERENCES [identity].[user](id),
	deleted_at DATETIME2 NULL,

	UNIQUE (tenant_id, campaign_code)
);

CREATE TABLE dbo.campaign_flight
(
	id INT PRIMARY KEY IDENTITY(1,1),
	campaign_id INT NOT NULL REFERENCES dbo.campaign(id),
	[start_date] DATE NOT NULL,
	end_date DATE NOT NULL,
	is_deleted BIT NOT NULL DEFAULT(0),
	deleted_by INT NULL REFERENCES [identity].[user](id),
	deleted_at DATETIME2 NULL
);

CREATE UNIQUE INDEX UQ_campaign_flight ON dbo.campaign_flight (campaign_id, [start_date]) WHERE is_deleted = 0;

CREATE TABLE dbo.campaign_flight_screen
(
	id INT PRIMARY KEY IDENTITY(1,1),
	campaign_flight_id INT NOT NULL REFERENCES dbo.campaign_flight(id),
	screen_id INT NOT NULL REFERENCES inv.screen(id),
	is_deleted BIT NOT NULL DEFAULT(0),
	deleted_by INT NULL REFERENCES [identity].[user](id),
	deleted_at DATETIME2 NULL
);

CREATE UNIQUE INDEX UQ_campaign_flight_screen ON dbo.campaign_flight_screen (campaign_flight_id, screen_id) WHERE is_deleted = 0;

CREATE TABLE dbo.campaign_screen_schedule
(
	id INT PRIMARY KEY IDENTITY(1,1),
	campaign_flight_screen_id INT NOT NULL REFERENCES dbo.campaign_flight_screen(id),
	day_of_week	NVARCHAR(50) NOT NULL,
	start_time TIME NOT NULL,
	end_time TIME NOT NULL,
	created_by INT NOT NULL REFERENCES [identity].[user](id),
	created_at DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
	modified_by INT NULL REFERENCES [identity].[user](id),
	modified_at DATETIME2 NULL,
	is_deleted BIT NOT NULL DEFAULT(0),
	deleted_by INT NULL REFERENCES [identity].[user](id),
	deleted_at DATETIME2 NULL
);

CREATE UNIQUE INDEX UQ_screen_schedule ON dbo.campaign_screen_schedule (campaign_flight_screen_id, day_of_week, start_time) WHERE is_deleted = 0;

CREATE TABLE dbo.campaign_playlist_item
(
	id INT PRIMARY KEY IDENTITY(1,1),
	schedule_id INT NOT NULL REFERENCES dbo.campaign_screen_schedule(id),
    media_id    INT NOT NULL REFERENCES dbo.media_library(id),
	play_order INT NOT NULL,
	duration_seconds INT NOT NULL DEFAULT(30),
	created_by  INT NOT NULL REFERENCES [identity].[user](id),
    created_at  DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
	is_deleted BIT NOT NULL DEFAULT(0),
	deleted_by INT NULL REFERENCES [identity].[user](id),
	deleted_at DATETIME2 NULL,

	CONSTRAINT UQ_playlist_order UNIQUE (schedule_id, play_order),
);
