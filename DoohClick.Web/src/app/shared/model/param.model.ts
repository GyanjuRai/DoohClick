export interface SelParamModel<T>
{
    filter?: T;
    offset?: number;
    pageSize?: number;
    sortBy?: string;
    sortOrder?: string;
}