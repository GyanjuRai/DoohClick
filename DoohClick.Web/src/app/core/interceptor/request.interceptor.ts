import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AuthService } from "../service/auth.service";

@Injectable({
    providedIn: 'root'
})

export class RequestInterceptor implements HttpInterceptor {
    
    constructor(
        private auth: AuthService
    ) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const request = this.getRequestWithHeaders(req);
        return this.sendRquest(request, next);
    }

    sendRquest(
        req: HttpRequest<any>,
        next: HttpHandler
    ) {
        return next.handle(req);
    }

    getRequestWithHeaders(req: HttpRequest<any>) {
        let headers = req.headers;
        const url = req.url.toLowerCase();
        const isLoginReq = url.includes('/login');

        if(!isLoginReq) {
            const token = this.auth.getAccessToken();
            if(token != '') {
                headers = headers.set('Authorization', `Bearer ${token}`);
            }
        }

        if(!(req.body instanceof FormData)) {
            headers = headers.set('Accept', 'application/json');
            const contentType = 'application/json; charset=utf-8;';
            headers = headers.set('Content-Type', contentType);

            if (headers.get('Content-Type' ) == 'angular/auto') {
                headers = headers.delete('Content-Type');
                headers = headers.set('Content-Type', contentType);
            }
        }
        
        return req.clone( { headers });
    }
}