-- ============================================================
-- DOOHClick — shared.listitem_category + shared.listitem seed
-- ============================================================
 
SET NOCOUNT ON;
 
-- ============================================================
-- CATEGORIES
-- ============================================================

INSERT INTO shared.listitem_category (category, [description]) VALUES
-- Player
('player_status',               'Operational status of a player device'),
('player_device_type',          'Hardware/OS type of the player device'),
-- Ruleset
('ruleset_status',              'Lifecycle status of a ruleset'),
-- Screen Group
('screen_group_group_type',     'How screens are logically grouped'),
-- Screen
('screen_type_category',     'Top-level screen type classification'),
('screen_type_subcategory',  'Finer classification within a screen type'),
('screen_type_orientation',  'Physical orientation of the display'),
-- Capability
('screen_capability_format',    'Supported media formats for playback'),
-- Location
('country',            'ISO 3166-1 alpha-2 country codes'),
('city',               'Cities where screens are deployed'),
('district',           'Districts / neighbourhoods within cities'),
('venue_type',         'Type of venue where screen is installed'),
-- Money
('currency_code',         'ISO 4217 currency codes');
GO

-- ============================================================
-- player_status
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Online',       'ONLINE',       (SELECT id FROM shared.listitem_category WHERE category = 'player_status')),
('Offline',      'OFFLINE',      (SELECT id FROM shared.listitem_category WHERE category = 'player_status')),
('Syncing',      'SYNCING',      (SELECT id FROM shared.listitem_category WHERE category = 'player_status')),
('Error',        'ERROR',        (SELECT id FROM shared.listitem_category WHERE category = 'player_status')),
('Rebooting',    'REBOOTING',    (SELECT id FROM shared.listitem_category WHERE category = 'player_status')),
('Provisioning', 'PROVISIONING', (SELECT id FROM shared.listitem_category WHERE category = 'player_status')),
('Decommissioned','DECOMMISSIONED',(SELECT id FROM shared.listitem_category WHERE category = 'player_status'));

-- ============================================================
-- player_device_type
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Android',         'ANDROID',          (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('BrightSign',      'BRIGHTSIGN',        (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('Chrome OS',       'CHROMEOS',          (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('Linux (x86)',     'LINUX_X86',         (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('Windows',         'WINDOWS',           (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('Raspberry Pi',    'RASPBERRY_PI',      (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('LG webOS',        'LG_WEBOS',          (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('Samsung Tizen',   'SAMSUNG_TIZEN',     (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type')),
('Apple tvOS',      'APPLE_TVOS',        (SELECT id FROM shared.listitem_category WHERE category = 'player_device_type'));

-- ============================================================
-- ruleset_status
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Draft',       'DRAFT',    (SELECT id FROM shared.listitem_category WHERE category = 'ruleset_status')),
('Active',      'ACTIVE',   (SELECT id FROM shared.listitem_category WHERE category = 'ruleset_status')),
('Inactive',    'INACTIVE', (SELECT id FROM shared.listitem_category WHERE category = 'ruleset_status')),
('Archived',    'ARCHIVED', (SELECT id FROM shared.listitem_category WHERE category = 'ruleset_status'));


-- ============================================================
-- screen_group_group_type
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Geographic',  'GEOGRAPHIC',   (SELECT id FROM shared.listitem_category WHERE category = 'screen_group_group_type')),
('Venue',       'VENUE',        (SELECT id FROM shared.listitem_category WHERE category = 'screen_group_group_type')),
('Route',       'ROUTE',        (SELECT id FROM shared.listitem_category WHERE category = 'screen_group_group_type')),
('Network',     'NETWORK',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_group_group_type')),
('Custom',      'CUSTOM',       (SELECT id FROM shared.listitem_category WHERE category = 'screen_group_group_type')),
('Demographic', 'DEMOGRAPHIC',  (SELECT id FROM shared.listitem_category WHERE category = 'screen_group_group_type'));

-- ============================================================
-- screen_type_category
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Billboard',           'BILLBOARD',        (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('In-Store',            'INSTORE',          (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Transit',             'TRANSIT',          (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Street Furniture',    'STREET_FURNITURE', (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Airport',             'AIRPORT',          (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Cinema',              'CINEMA',           (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Healthcare',          'HEALTHCARE',       (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Sports Venue',        'SPORTS_VENUE',     (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Office Building',     'OFFICE',           (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category')),
('Education',           'EDUCATION',        (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_category'));

-- ============================================================
-- screen_type_subcategory
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
-- Billboard subs
('Large Format',            'LARGE_FORMAT',         (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Super Large Format',      'SUPER_LARGE_FORMAT',   (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Roadside Static',         'ROADSIDE_STATIC',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Highway Billboard',       'HIGHWAY',              (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
-- In-Store subs
('Checkout Display',        'CHECKOUT',             (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Aisle Display',           'AISLE',                (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Entrance Display',        'ENTRANCE',             (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Window Display',          'WINDOW',               (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Shelf Edge',              'SHELF_EDGE',            (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
-- Transit subs
('Bus Shelter',             'BUS_SHELTER',           (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Train Platform',          'TRAIN_PLATFORM',        (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Metro/Subway',            'METRO',                 (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('On-Vehicle',              'ON_VEHICLE',            (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
-- Airport subs
('Arrivals Hall',           'ARRIVALS',              (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Departures Hall',         'DEPARTURES',            (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Gate Lounge',             'GATE_LOUNGE',           (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Baggage Claim',           'BAGGAGE_CLAIM',         (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
-- Street Furniture subs
('Bus Stop Panel',          'BUS_STOP_PANEL',        (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Kiosk',                   'KIOSK',                 (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Pillar / Column',         'PILLAR',                (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
-- Gym / Health subs
('Gym Floor',               'GYM_FLOOR',             (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory')),
('Reception / Lobby',       'RECEPTION',             (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_subcategory'));

-- ============================================================
-- screen_type_orientation
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Landscape',   'LANDSCAPE',    (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_orientation')),
('Portrait',    'PORTRAIT',     (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_orientation')),
('Tilt',        'TILT',         (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_orientation')),
('Square',      'SQUARE',       (SELECT id FROM shared.listitem_category WHERE category = 'screen_type_orientation'));

-- ============================================================
-- screen_capability_format
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('MP4 (H.264)',      'MP4',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('MP4 (H.265/HEVC)', 'MP4_HEVC', (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('WebM (VP9)',       'WEBM',     (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('MOV',             'MOV',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('AVI',             'AVI',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('JPEG',            'JPG',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('PNG',             'PNG',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('GIF',             'GIF',      (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('HTML5',           'HTML5',    (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format')),
('ZIP (HTML bundle)','ZIP_HTML', (SELECT id FROM shared.listitem_category WHERE category = 'screen_capability_format'));

 
-- ============================================================
-- country 
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
-- South Asia
('Nepal',           'NP', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('India',           'IN', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Bangladesh',      'BD', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Sri Lanka',       'LK', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Pakistan',        'PK', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
-- Southeast Asia
('Singapore',       'SG', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Malaysia',        'MY', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Thailand',        'TH', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Indonesia',       'ID', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Philippines',     'PH', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Vietnam',         'VN', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
-- East Asia
('Japan',           'JP', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('South Korea',     'KR', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('China',           'CN', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
-- Middle East
('UAE',             'AE', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Saudi Arabia',    'SA', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Qatar',           'QA', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
-- Western
('United States',   'US', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('United Kingdom',  'GB', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Australia',       'AU', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Germany',         'DE', (SELECT id FROM shared.listitem_category WHERE category = 'country')),
('Canada',          'CA', (SELECT id FROM shared.listitem_category WHERE category = 'country'));


-- ============================================================
-- city 
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
-- Nepal
('Kathmandu',       'KTM',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Pokhara',         'PKR',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Lalitpur',        'LTP',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Bhaktapur',       'BKT',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Biratnagar',      'BRT',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Birgunj',         'BRG',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Butwal',          'BTW',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Dharan',          'DHR',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Hetauda',         'HTD',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Nepalgunj',       'NPG',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
-- India
('Mumbai',          'BOM',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Delhi',           'DEL',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Bangalore',       'BLR',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Chennai',         'MAA',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Hyderabad',       'HYD',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
-- SEA
('Singapore',       'SIN',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Kuala Lumpur',    'KUL',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Bangkok',         'BKK',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Jakarta',         'JKT',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
-- Middle East
('Dubai',           'DXB',          (SELECT id FROM shared.listitem_category WHERE category = 'city')),
('Abu Dhabi',       'AUH',          (SELECT id FROM shared.listitem_category WHERE category = 'city'));


-- ============================================================
-- district  (Kathmandu Valley — MVP focus)
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
-- Kathmandu districts / areas
('New Baneshwor',       'NEW_BANESHWOR',     (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Old Baneshwor',       'OLD_BANESHWOR',     (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Thamel',              'THAMEL',            (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Putalisadak',         'PUTALISADAK',       (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Koteshwor',           'KOTESHWOR',         (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Kalanki',             'KALANKI',           (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Balaju',              'BALAJU',            (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Chabahil',            'CHABAHIL',          (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Maharajgunj',         'MAHARAJGUNJ',       (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Durbarmarg',          'DURBARMARG',        (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('New Road',            'NEW_ROAD',          (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Sundhara',            'SUNDHARA',          (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Tripureshwor',        'TRIPURESHWOR',      (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Lazimpat',            'LAZIMPAT',          (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Naxal',               'NAXAL',             (SELECT id FROM shared.listitem_category WHERE category = 'district')),
-- Lalitpur
('Pulchowk',            'PULCHOWK',          (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Jawalakhel',          'JAWALAKHEL',        (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Patan',               'PATAN',             (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Kumaripati',          'KUMARIPATI',        (SELECT id FROM shared.listitem_category WHERE category = 'district')),
-- Bhaktapur
('Suryabinayak',        'SURYABINAYAK',      (SELECT id FROM shared.listitem_category WHERE category = 'district')),
('Lokanthali',          'LOKANTHALI',        (SELECT id FROM shared.listitem_category WHERE category = 'district'));

-- ============================================================
-- venue_type
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
('Shopping Mall',           'MALL',             (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Supermarket / Grocery',   'SUPERMARKET',      (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Convenience Store',       'CONVENIENCE',      (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Airport — International', 'AIRPORT_INTL',     (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Airport — Domestic',      'AIRPORT_DOM',      (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Bus Terminal',            'BUS_TERMINAL',      (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Train / Metro Station',   'TRAIN_STATION',     (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Highway / Roadside',      'HIGHWAY',           (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Street / Pavement',       'STREET',            (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Petrol Station',          'PETROL_STATION',    (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Gym / Fitness Centre',    'GYM',               (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Hospital / Clinic',       'HOSPITAL',          (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Hotel Lobby',             'HOTEL',             (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Restaurant / Food Court',  'RESTAURANT',       (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Bar / Nightclub',         'BAR',               (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Cinema / Theatre',        'CINEMA',            (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Sports Stadium',          'STADIUM',           (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('University / College',    'UNIVERSITY',        (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Office Building',         'OFFICE',            (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Residential Complex',     'RESIDENTIAL',       (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Government Building',     'GOVERNMENT',        (SELECT id FROM shared.listitem_category WHERE category = 'venue_type')),
('Bank / Financial',        'BANK',              (SELECT id FROM shared.listitem_category WHERE category = 'venue_type'));

-- ============================================================
-- currency_code  (ISO 4217)
-- ============================================================
INSERT INTO shared.listitem (item, [value], category_id) VALUES
-- South Asia
('NPR',          'NPR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('INR',            'INR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('BDT',        'BDT', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('LKR',        'LKR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('PKR',         'PKR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
-- Southeast Asia
('SGD',        'SGD', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('MYR',       'MYR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('THB',       'THB', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('IDR',       'IDR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('PHP',         'PHP', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('VND',         'VND', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
-- East Asia
('JPY',            'JPY', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('KRW',        'KRW', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('CNY',            'CNY', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
-- Middle East
('AED',              'AED', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('SAR',             'SAR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('QAR',            'QAR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
-- Western
('USD',               'USD', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('GBP',           'GBP', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('EUR',                    'EUR', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('AUD',       'AUD', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code')),
('CAD',         'CAD', (SELECT id FROM shared.listitem_category WHERE category = 'currency_code'));