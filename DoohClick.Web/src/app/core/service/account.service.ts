import { Injectable } from "@angular/core";
import { WebApiService } from "../../shared/service/web-api.service";
import { Observable } from "rxjs";
import { MvResponse } from "../../shared/model/response.model";
import { MvLoginInfoParam, MvLoginResponse, MvRefreshTokenParam } from "../model/account.model";

@Injectable({providedIn : 'root'})

export class AccountService {

    constructor(
        private api: WebApiService
    ) {

    }

    login(param: MvLoginInfoParam): Observable<MvResponse<MvLoginResponse>> {
        return this.api.post('account/login', param);
    }

    refreshToken(param: MvRefreshTokenParam): Observable<MvResponse<MvLoginResponse>> {
        return this.api.post('account/refresh-token', param)
    }
}