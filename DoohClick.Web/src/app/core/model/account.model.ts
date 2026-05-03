export interface MvLoginInfoParam
{
    userName: string;
    password: string;
    tenantCode?: string;
}

export interface MvRefreshTokenParam
{
    accessToken: string;
    refreshToken: string;
    userId: number;
}

export interface MvLoginResponse
{
    accessToken : string;
    expiredAt : Date;
    refreshToken : string;
}

export interface MvRefreshTokenParam
{
    accessToken: string;
    refreshToken: string;
    userId: number;
}