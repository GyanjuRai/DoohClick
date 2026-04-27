
-- listitem_category
INSERT INTO shared.listitem_category (tenant_id, category, category_code) VALUES
(NULL, 'Campaign Status',       'CAMPAIGN_STATUS'),
(NULL, 'Media Type',            'MEDIA_TYPE'),
(NULL, 'Country Code',          'COUNTRY_CODE'),
(NULL, 'City',                  'CITY'),
(NULL, 'Timezone',              'TIMEZONE'),
(NULL, 'Default Resolution',    'DEFAULT_RESOLUTION'),
(NULL, 'Orientation',           'ORIENTATION'),
(NULL, 'User Role',             'USER_ROLE'),
(NULL, 'Currency',              'CURRENCY'),
(NULL, 'Media Status',          'MEDIA_STATUS'),
(NULL, 'Day Of Week',           'DAY_OF_WEEK'),
(NULL, 'Audience Source',       'AUDIENCE_SOURCE');

INSERT INTO shared.listitem_category (tenant_id, category, category_code) VALUES
(NULL, 'Country',   'COUNTRY'),
(NULL, 'City',      'CITY'),
(NULL, 'Timezone',  'TIMEZONE');


-- Campaign Status
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active) 
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Draft',       'DRAFT'),
    ('Scheduled',   'SCHEDULED'),
    ('Active',      'ACTIVE'),
    ('Cancelled',   'CANCELLED'),
    ('Completed',   'COMPLETED')
) AS v(item, code)
WHERE category_code = 'CAMPAIGN_STATUS';

-- Media Type
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Image',   'IMAGE'),
    ('Video',   'VIDEO'),
    ('HTML',    'HTML')
) AS v(item, code)
WHERE category_code = 'MEDIA_TYPE';

-- Media Status
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Pending',     'PENDING'),
    ('Ready',       'READY')
) AS v(item, code)
WHERE category_code = 'MEDIA_STATUS';

-- Orientation
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Landscape',   'LANDSCAPE'),
    ('Portrait',    'PORTRAIT'),
    ('Square', 'SQUARE')
) AS v(item, code)
WHERE category_code = 'ORIENTATION';

-- Default Resolution
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('1920x1080',   '1920X1080'),
    ('1080x1920',   '1080X1920'),
    ('1280x720',    '1280X720'),
    ('3840x2160',   '3840X2160'),
    ('2560x1440',   '2560X1440')
) AS v(item, code)
WHERE category_code = 'DEFAULT_RESOLUTION';

-- User Role
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Admin',       'ADMIN'),
    ('Manager',     'MANAGER'),
    ('Operator',    'OPERATOR')
) AS v(item, code)
WHERE category_code = 'USER_ROLE';

-- Currency
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('US Dollar',           'USD'),
    ('Euro',                'EUR'),
    ('British Pound',       'GBP'),
    ('Japanese Yen',        'JPY'),
    ('Nepalese Rupee',      'NPR'),
    ('Indian Rupee',        'INR'),
    ('Australian Dollar',   'AUD'),
    ('Canadian Dollar',     'CAD'),
    ('Swiss Franc',         'CHF'),
    ('Singapore Dollar',    'SGD')
) AS v(item, code)
WHERE category_code = 'CURRENCY';

-- Day Of Week
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Monday',      'MON'),
    ('Tuesday',     'TUE'),
    ('Wednesday',   'WED'),
    ('Thursday',    'THU'),
    ('Friday',      'FRI'),
    ('Saturday',    'SAT'),
    ('Sunday',      'SUN')
) AS v(item, code)
WHERE category_code = 'DAY_OF_WEEK';

-- Audience Source
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Manual',          'MANUAL'),
    ('Footfall',        'FOOTFALL'),
    ('Traffic Count',   'TRAFFIC_COUNT'),
    ('Sensor',          'SENSOR')
) AS v(item, code)
WHERE category_code = 'AUDIENCE_SOURCE';

-- Country
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Nepal',           'NP'),
    ('Sweden',          'SE'),
    ('Japan',           'JP'),
    ('United Kingdom',  'GB'),
    ('United States',   'US'),
    ('South Korea',     'KR')
) AS v(item, code)
WHERE category_code = 'COUNTRY';

-- City
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Kathmandu',   'KTM'),
    ('Pokhara',     'PKR'),
    ('Lalitpur',    'LTP'),
    ('Stockholm',   'STO'),
    ('Gothenburg',  'GOT'),
    ('Tokyo',       'TYO'),
    ('Osaka',       'OSA'),
    ('London',      'LON'),
    ('Manchester',  'MAN'),
    ('New York',    'NYC'),
    ('Los Angeles', 'LAX'),
    ('Chicago',     'CHI'),
    ('Seoul',       'SEL'),
    ('Busan',       'PUS')
) AS v(item, code)
WHERE category_code = 'CITY';

-- Timezone
INSERT INTO shared.listitem (tenant_id, category_id, item, code, is_active)
SELECT NULL, id, item, code, 1 FROM shared.listitem_category,
(VALUES
    ('Nepal Time',          'Asia/Kathmandu'),
    ('Central European',    'Europe/Stockholm'),
    ('Japan Standard',      'Asia/Tokyo'),
    ('Greenwich Mean',      'Europe/London'),
    ('Eastern Time',        'America/New_York'),
    ('Central Time',        'America/Chicago'),
    ('Mountain Time',       'America/Denver'),
    ('Pacific Time',        'America/Los_Angeles'),
    ('Korea Standard',      'Asia/Seoul')
) AS v(item, code)
WHERE category_code = 'TIMEZONE';