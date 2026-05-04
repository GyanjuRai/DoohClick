import { GridColumn } from '../../../shared/model/grid-config.model';

export const advertiserColumn = [
  { name: 'action', displayName: 'Action', type: 'action', width: '4rem' },
  { name: 'name', displayName: 'Advertiser', type: 'text', width: '14rem' },
  { name: 'contactName', displayName: 'Contact', type: 'text', width: '10rem' },
  { name: 'contactEmail', displayName: 'Email', type: 'text', width: '14rem' },
  { name: 'contactPhone', displayName: 'Phone', type: 'text', width: '9rem' },
  { name: 'isActive', displayName: 'Status', type: 'badge', width: '7rem' },
  { name: 'creator', displayName: 'Created By', type: 'text', width: '8rem' },
  { name: 'createdAt', displayName: 'Created', type: 'date', width: '8rem' },
] as GridColumn[];
