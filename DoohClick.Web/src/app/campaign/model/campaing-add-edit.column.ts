import { GridColumn } from "../../shared/model/grid-config.model";

export const campaignAddEditColumn = [
    {
        name: 'startDate',
        displayName: 'Start Date',
        type: 'date'
    },
    {
        name: 'endDate',
        displayName: 'End Date',
        type: 'date'
    },
    {
        name: 'screens',
        displayName: 'Screens',
        type: 'text'
    },
    {
        name: 'action',
        displayName: '',
        type: 'action'
    }
] as GridColumn[];