import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { delay, Observable, retry } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class WebApiService {
  private apiUrl: String;

  constructor(private http: HttpClient) {
    this.apiUrl = 'http://localhost:5200/';
  }

  get(url: string, param?: any, nestedParam = false): Observable<any> {
    let params = {};
    if (nestedParam) {
      this.buildHttpParams(params, param, '');
    } else {
      params = param as HttpParams;
    }

    return this.http
      .get(`${this.apiUrl}${url}`, { params: params, withCredentials: true })
      .pipe(delay(100), retry(0));
  }

  post(url: string, param: any): Observable<any> {
    return this.http
      .post(`${this.apiUrl}${url}`, param, { withCredentials: true })
      .pipe(retry(0));
  }

  put(url: string, param: any): Observable<any> {
    return this.http
      .put(`${this.apiUrl}${url}`, param, { withCredentials: true })
      .pipe(retry(0));
  }

  delete(url: string, param?: any): Observable<any> {
    const options = param ? { body: param } : {};
    return this.http.delete(`${this.apiUrl}${url}`, options).pipe(retry(0));
  }

  private buildHttpParams(params: any, data: any, currentPath: string) {
    Object.keys(data).forEach((key) => {
      if (data[key] instanceof Object && !(data[key] instanceof Array)) {
        this.buildHttpParams(params, data[key], `${currentPath}${key}.`);
      } else {
        params[`${currentPath}${key}`] = data[key];
      }
    });
  }
}
