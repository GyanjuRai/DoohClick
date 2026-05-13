import { GridColumn } from '../../shared/model/grid-config.model';

export const campaignScreenColumn = [
  { name: 'dayOfWeek', displayName: 'Day', type: 'text', width: '140px' },
  { name: 'timeSlot', displayName: 'Time slot', type: 'text', width: '160px' },
  { name: 'playlist', displayName: 'Playlist', type: 'badge' },
] as GridColumn[];
