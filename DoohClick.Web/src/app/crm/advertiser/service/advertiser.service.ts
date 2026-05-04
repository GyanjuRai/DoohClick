import { Injectable } from "@angular/core";
import { WebApiService } from "../../../shared/service/web-api.service";
import { MvGridParamOption } from "../../../shared/model/param.model";
import { MvAdvertiser, MvAdvertiserDdl, MvAdvertiserFilterOptions, MvTenantIdParam } from "../model/advertiser.model";
import { Observable } from "rxjs";
import { MvGridResponse, MvResponse } from "../../../shared/model/response.model";

@Injectable({
    providedIn: 'root'
})

export class AdvertiserService {

    constructor(
        private api: WebApiService
    )
    {

    }

    getGrid(param: MvGridParamOption<MvAdvertiserFilterOptions>): Observable<MvResponse<MvGridResponse<MvAdvertiser>>> {
        return this.api.get('crm/advertiser/grid', param, true);
    }

    getDdl(param: MvTenantIdParam): Observable<MvResponse<MvAdvertiserDdl[]>> {
        return this.api.get('crm/advertiser/ddl', param);
    }
}