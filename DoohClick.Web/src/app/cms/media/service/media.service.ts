import { Injectable } from "@angular/core";
import { WebApiService } from "../../../shared/service/web-api.service";
import { MvGridParamOption } from "../../../shared/model/param.model";
import { MvMedia, MvMediaDdl, MvMediaDel, MvMediaFilterOptions } from "../model/media.model";
import { Observable } from "rxjs";
import { MvGridResponse, MvResponse } from "../../../shared/model/response.model";
import { MvFileUploadParam } from "../../../shared/model/file.model";
import { MvTenantIdParam } from "../../../crm/advertiser/model/advertiser.model";

@Injectable({
    providedIn: 'root'
})

export class MediaService {

    constructor(
        private api: WebApiService
    )
    {}

    getGrid(param: MvGridParamOption<MvMediaFilterOptions>): Observable<MvResponse<MvGridResponse<MvMedia>>> {
        return this.api.get('cms/media/grid', param, true);
    }

    add(param: MvMedia): Observable<MvResponse<MvMedia>> {
        return this.api.post('cms/media', param);
    }

    remove(param: MvMediaDel): Observable<MvResponse<MvMedia>> {
        return this.api.delete('cms/media', param);
    }

    getDdl(param: MvTenantIdParam): Observable<MvResponse<MvMediaDdl[]>> {
        return this.api.get('cms/media/ddl', param);
    }
}