import { GridColumn } from '../../../shared/model/grid-config.model';

export const screenAddEditColumn = [
  {
    name: 'dayOfWeek',
    displayName: 'Day',
    type: 'text',
  },
  {
    name: 'openTime',
    displayName: 'Open time',
    type: 'date',
  },
  {
    name: 'closeTime',
    displayName: 'Close time',
    type: 'date',
  },
  {
    name: 'estimatedImpression',
    displayName: 'Audience',
    type: 'text',
  },
  {
    name: 'audienceSource',
    displayName: 'Audience source',
    type: 'text',
  },
  { name: 'action', displayName: 'action', type: 'action' },
] as GridColumn[];
