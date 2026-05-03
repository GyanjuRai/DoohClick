import { inject } from '@angular/core';
import { Router, RouterStateSnapshot, Routes } from '@angular/router';
import { AuthGuardService } from './core/guard/auth-guard.guard';
import { ROUTE_PATHS } from './shared';
import { AuthService } from './core/service/auth.service';

export const appRoutes = [
  {
    path: '',
    loadChildren: () =>
      import('./layout/layout.module').then((m) => m.LayoutModule),
    canActivate: [
      (state: RouterStateSnapshot) =>
        inject(AuthGuardService).canActivate(state),
    ],
  },
  {
    path: ROUTE_PATHS.AUTH,
    loadChildren: () =>
      import('./layout/auth-layout/auth-layout.module').then(
        (m) => m.AuthLayoutModule,
      ),
    canActivate: [
      () => {
        const auth = inject(AuthService);
        const router = inject(Router);

        if(auth.isAuthenticated()) {
          router.navigate([ROUTE_PATHS.SCREEN_HOME]);
          return false;
        }
        return true;
      }
    ]
  },
  {
    path: '**',
    redirectTo: '404',
    pathMatch: 'full',
  },
] as Routes;
