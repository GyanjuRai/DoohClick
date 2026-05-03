import { Injectable } from "@angular/core";
import { WebApiService } from "./web-api.service";
import { Observable } from "rxjs";
import { MvListitemDdl, MvResponse } from "../model/response.model";
import { MvListitemDdlParam } from "../model/param.model";

@Injectable({
    providedIn: 'root'
})

export class ListitemService {

    constructor(private api: WebApiService) {}

    getDdl(param: MvListitemDdlParam): Observable<MvResponse<MvListitemDdl[]>> {
        return this.api.get('reference/list-item/ddl', param);
    }

}