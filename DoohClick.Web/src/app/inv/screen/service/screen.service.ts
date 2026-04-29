import { Injectable } from "@angular/core";
import { WebApiService } from "../../../shared/service/web-api.service";
import { Observable } from "rxjs";
import { MvGridResponse } from "../../../shared/model/response.model";
import { MvGridParamOption } from "../../../shared/model/param.model";
import { MvScreenFilterOptions } from "../model/screen.model";

@Injectable({ providedIn: 'root' })

export class ScreenService {

    constructor(
        private api: WebApiService
    )
    {}

    getGird(param: MvGridParamOption<MvScreenFilterOptions> ) : Observable<MvGridResponse<[]>> {
        return this.api.get('inv/screen/grid', param);
    }

    save() {
        return this.api.post('', {});
    }

    remove() {
        return this.api.delete('');
    }
}