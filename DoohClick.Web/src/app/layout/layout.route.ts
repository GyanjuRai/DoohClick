import { Routes } from '@angular/router';
import { MainLayoutComponent } from './main-layout/component/main-layout.component';
import { ROUTE_PATHS } from '../shared/routing/route-paths.const';

export const layoutRoutes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {
        path: ROUTE_PATHS.SCREEN,
        loadChildren: () =>
          import('../inv/inv.module').then(
            (c) => c.InvModule,
          ),
      },
      {
        path: ROUTE_PATHS.CAMPAIGN,
        loadChildren: () =>
          import('../campaign/campaign.module').then(
            (c) => c.CampaignModule,
          )
      },
      {
        path: ROUTE_PATHS.ADVERTISER,
        loadChildren: () =>
          import('../crm/crm.module').then(
            (c) => c.CrmModule,
          )
      },
      {
        path: '',
        redirectTo: ROUTE_PATHS.SCREEN,
        pathMatch: 'full',
      },
    ],
  },
] as Routes;
