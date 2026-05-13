import { GridColumn } from '../../shared/model/grid-config.model';

export const campaignColumns: GridColumn[] = [
  { name: 'actions', displayName: 'Actions', width: '100px', type: 'action' },
  { name: 'campaignCode', displayName: 'Code', width: '150px', type: 'text' },
  { name: 'name', displayName: 'Campaign', width: '200px', type: 'text' },
  {
    name: 'advertiser',
    displayName: 'Advertiser',
    width: '160px',
    type: 'text',
  },
  {
    name: 'startDate',
    displayName: 'Start Date',
    width: '130px',
    type: 'date',
  },
  { name: 'endDate', displayName: 'End Date', width: '130px', type: 'date' },
  {
    name: 'durationInDays',
    displayName: 'Duration days',
    width: '120px',
    type: 'text',
  },
  { name: 'creator', displayName: 'Created By', width: '140px', type: 'text' },
  {
    name: 'createdAt',
    displayName: 'Created At',
    width: '150px',
    type: 'date',
  },
];
