import { Injectable, Injector } from '@angular/core';
import { ROUTE_PATHS } from '../../shared';
import { Router, RouterStateSnapshot } from '@angular/router';
import { AppComponent } from '../../app.component';
import { AuthService } from '../service/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuardService extends AppComponent {
  loginUrl = `${ROUTE_PATHS.AUTH}/${ROUTE_PATHS.LOGIN}`;

  constructor(
    private injector: Injector,
    private router: Router
  ) {
    super(injector);
  }

  canActivate(state: RouterStateSnapshot): boolean {
    const url: string = state.url;
    return this.checkLogin(url);
  }

  canActivateChild(state: RouterStateSnapshot): boolean {
    return this.canActivate(state);
  }

  checkLogin(url: string): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate([this.loginUrl], {
        queryParams: { returnUrl: url },
      });
      this.showToast('info', 'Login Required', 'Please login to continue.');
      return false;
    }

    return true;
  }
}
