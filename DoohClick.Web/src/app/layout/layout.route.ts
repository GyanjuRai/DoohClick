import { ActivatedRouteSnapshot, Routes } from '@angular/router';
import { MainLayoutComponent } from './main-layout/component/main-layout.component';
import { ROUTE_PATHS } from '../shared/routing/route-paths.const';
import { inject } from '@angular/core';
import { RoleGuardService } from '../core/guard/role-guard.guard';
import { UserRole } from '../core/enum/user-role.enum';

export const layoutRoutes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivateChild: [
      (route: ActivatedRouteSnapshot) =>
        inject(RoleGuardService).canActivate(route),
    ],
    children: [
      {
        path: ROUTE_PATHS.SCREEN,
        loadChildren: () =>
          import('../inv/inv.module').then((c) => c.InvModule),
        data: { roles: [UserRole.Admin, UserRole.Manager, UserRole.Operator] },
      },
      {
        path: ROUTE_PATHS.CAMPAIGN,
        loadChildren: () =>
          import('../campaign/campaign.module').then((c) => c.CampaignModule),
        data: { roles: [UserRole.Admin, UserRole.Manager, UserRole.Operator] },
      },
      {
        path: ROUTE_PATHS.ADVERTISER,
        loadChildren: () =>
          import('../crm/crm.module').then((c) => c.CrmModule),
        data: { roles: [UserRole.Admin, UserRole.Manager] },
      },
      {
        path: ROUTE_PATHS.MEDIA,
        loadChildren: () =>
          import('../cms/cms.module').then((c) => c.CmsModule),
        data: { roles: [UserRole.Admin, UserRole.Manager, UserRole.Operator] },
      },
      {
        path: '',
        redirectTo: ROUTE_PATHS.SCREEN,
        pathMatch: 'full',
      },
    ],
  },
] as Routes;
