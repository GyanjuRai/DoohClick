
export interface MvGridResponse<T> {
    data?: T[];
    totalRows: number;
}

export interface MvResponse<T> {
    type: string;
    message: string;
    data?: T
}