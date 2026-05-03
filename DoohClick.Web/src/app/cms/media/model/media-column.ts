import { GridColumn } from '../../../shared/model/grid-config.model';

export const mediaColumn: GridColumn[] = [
  { name: 'preview', displayName: 'Preview', width: '5rem', type: 'preview' },
  { name: 'displayName', displayName: 'Name', width: '12rem', type: 'text' },
  { name: 'fileName', displayName: 'File name', width: '12rem', type: 'text' },
  {
    name: 'resolution',
    displayName: 'Resolution',
    width: '7rem',
    type: 'text',
  },
  {
    name: 'durationSec',
    displayName: 'Duration',
    width: '6rem',
    type: 'number',
  },
  { name: 'isVideo', displayName: 'Type', width: '5rem', type: 'badge' },
  { name: 'status', displayName: 'Status', width: '6rem', type: 'badge' },
  { name: 'createdAt', displayName: 'Created', width: '10rem', type: 'date' },
  { name: 'action', displayName: '', width: '5rem', type: 'action' },
];
