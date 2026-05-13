/**
 * Centralized route path constants
 * Use these throughout the app instead of hardcoding paths
 */

export const ROUTE_PATHS = {
  ROOT: '',
  
  LAYOUT: '',

  AUTH: 'auth',
  LOGIN: 'login',
  AUTH_LOGIN: 'auth/login',

  SCREEN: 'screen',
  SCREEN_LIST: 'list',
  SCREEN_HOME: 'screen/list',

  CAMPAIGN: 'campaign',
  CAMPAIGN_DRAFT: 'drafts',
  CAMPAIGN_SCHEDULED: 'scheduled',
  CAMPAIGN_ACTIVE: 'active',
  CAMPAIGN_COMPLETED: 'completed',
  CAMPAIGN_CANCELLED: 'cancelled',

  CAMPAIGN_MEDIA: 'campaign-media',
  
  ADVERTISER: 'advertiser',
  ADVERTISER_LIST: 'list',

  PLAYLIST: 'playlist',
  PLAYLIST_LIST: 'list',

  MEDIA: 'media',
  MEDIA_LIST: 'list',

  NOT_FOUND: '404',
  WILDCARD: '**',
};
