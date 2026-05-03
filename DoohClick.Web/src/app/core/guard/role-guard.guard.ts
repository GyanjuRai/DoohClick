import { Injectable, Injector } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from '../service/auth.service';
import { UserRole } from '../enum/user-role.enum';
import { ROUTE_PATHS } from '../../shared';
import { AppComponent } from '../../app.component';

@Injectable({
  providedIn: 'root',
})
export class RoleGuardService extends AppComponent {
  constructor(
    private injector: Injector,
  ) {
    super(injector);
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const allowedRoles: UserRole[] = route.data['roles'];
    const userRole = this.auth.getUserRole();

    if (!userRole) {
      this.auth.navigate([ROUTE_PATHS.AUTH_LOGIN]);
      return false;
    }

    if (allowedRoles.includes(userRole)) return true;

    this.redirectToDashboard(userRole);
    return false;
  }

  private redirectToDashboard(role: string): void {
    this.showToast(
      'warn',
      'Access Denied',
      'You do not have permission to view this page.',
    );

    switch (role) {
      case UserRole.Admin:
        this.auth.navigate([ROUTE_PATHS.SCREEN_HOME]);
        break;
      case UserRole.Manager:
        this.auth.navigate([ROUTE_PATHS.SCREEN_HOME]);
        break;
      case UserRole.Operator:
        this.auth.navigate([ROUTE_PATHS.SCREEN_HOME]);
        break;
      default:
        this.auth.navigate([ROUTE_PATHS.SCREEN_HOME]);
    }
  }
}
