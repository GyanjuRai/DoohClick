import { Injectable } from "@angular/core";
import { WebApiService } from "./web-api.service";
import { MvFileUploadParam, MvFileUploadResult } from "../model/file.model";
import { Observable } from "rxjs";
import { MvResponse } from "../model/response.model";

@Injectable({
    providedIn: 'root'
})

export class FileService {

    constructor(
        private api: WebApiService
    )
    {}

    upload(param: FormData): Observable<MvResponse<MvFileUploadResult>> {
        return this.api.post('file/upload', param);
    }
}