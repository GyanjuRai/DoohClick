import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable, Injector } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  filter,
  Observable,
  switchMap,
  take,
  throwError,
} from 'rxjs';
import { AppComponent } from '../../app.component';
import { ROUTE_PATHS } from '../../shared';
import { MvLoginResponse, MvRefreshTokenParam } from '../model/account.model';
import { AccountService } from '../service/account.service';
import { MvResponse } from '../../shared/model/response.model';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorInterceptor
  extends AppComponent
  implements HttpInterceptor
{
  private isRefreshing = false;
  private refreshSubject = new BehaviorSubject<string | null>(null);

  constructor(
    private accountService: AccountService,
    private injector: Injector,
  ) {
    super(injector);
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401(req, next);
        }

        return throwError(() => error);
      }),
    );
  }

  private handle401(
    req: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    if (this.isRefreshing) {
      return this.refreshSubject.pipe(
        filter((token) => token !== null),
        take(1),
        switchMap((token) =>
          next.handle(
            req.clone({
              setHeaders: { Authorization: `Bearer ${token}` },
            }),
          ),
        ),
      );
    }

    this.isRefreshing = true;
    this.refreshSubject.next(null);

    const payload = {
      accessToken: this.auth.getAccessToken(),
      refreshToken: this.auth.getRefreshToken(),
      userId: this.auth.getUserId(),
    } as MvRefreshTokenParam;

    return this.accountService.refreshToken(payload).pipe(
      switchMap((response: MvResponse<MvLoginResponse>) => {
        this.isRefreshing = false;
        this.auth.setSession(response.data!);
        this.refreshSubject.next(response.data!.accessToken);
        return next.handle(
          req.clone({
            setHeaders: {
              Authorization: `Bearer ${response.data?.accessToken}`,
            },
          }),
        );
      }),
      catchError(() => {
        this.isRefreshing = false;
        this.forceLogout();
        return throwError(() => new Error('Session expired'));
      }),
    );
  }

  private forceLogout(): void {
    this.auth.clearAuth();
    this.showToast('warn', 'Session expired.', 'Please log in again.');
    this.auth.navigate([ROUTE_PATHS.AUTH_LOGIN]);
  }
}
