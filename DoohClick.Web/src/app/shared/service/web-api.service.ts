import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, retry } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class WebApiService {
    private apiUrl: String

    constructor(private http: HttpClient) 
    {
        this.apiUrl = "http://localhost:5200/api/";
    }


    get(url: string, param?: any) : Observable<any> {

        let options = {
            withCredentials: true,
            ...(param && { params: param as HttpParams})
        };
        return this.http.get(`${this.apiUrl}${url}`, options).pipe(retry(0));
    }

    post(url: string, param: any) : Observable<any> {
        return this.http.post(`${this.apiUrl}${url}`, param, { withCredentials: true }).pipe(retry(0));
    }

    put(url: string, param: any) : Observable<any> {
        return this.http.put(`${this.apiUrl}${url}`, param, { withCredentials: true }).pipe(retry(0));
    }

    delete(url: string, param: any) : Observable<any> {
        
        return this.http.delete( url = `${this.apiUrl}${url}`, { withCredentials: true, body: param }).pipe(retry(0));
    }
}