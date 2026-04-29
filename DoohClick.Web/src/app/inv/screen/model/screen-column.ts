import { GridColumn } from '../../../shared/model/grid-config.model';

export const screenColumn = [
  { name: 'action', displayName: 'Action', width: '100px', type: 'action' },
  {
    name: 'screenCode',
    displayName: 'Screen Code',
    width: '150px',
    type: 'text',
  },
  { name: 'name', displayName: 'Name', width: '170px', type: 'text' },
  { name: 'location', displayName: 'Location', width: '160px', type: 'text' },
  { name: 'countryCode', displayName: 'Country', width: '110px', type: 'text' },
  { name: 'city', displayName: 'City', width: '120px', type: 'text' },
  {
    name: 'orientation',
    displayName: 'Orientation',
    width: '120px',
    type: 'badge',
  },
  {
    name: 'defaultResolution',
    displayName: 'Resolution',
    width: '120px',
    type: 'text',
  },
  { name: 'isActive', displayName: 'Status', width: '90px', type: 'badge' },
  {
    name: 'createdAt',
    displayName: 'Created At',
    width: '140px',
    type: 'date',
  },
] as GridColumn[];
