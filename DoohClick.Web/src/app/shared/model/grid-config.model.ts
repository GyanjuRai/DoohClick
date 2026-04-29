
export interface GridConfig {
    column: GridColumn[],
    dataSource: {
        data: [],
        totalRows: number
    }
    options: {
        filter?: any;
        offset?: number;
        pageSize?: number;
        searchText?: string;
    } 
}

export interface GridColumn {
    name: string;
    displayName: string;
}
