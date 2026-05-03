
export interface GridConfig {
    column: GridColumn[],
    dataSource: {
        data: any[],
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
  width?: string;
  type: 'text' | 'number' | 'date' | 'badge' | 'currency' | 'action';
}
