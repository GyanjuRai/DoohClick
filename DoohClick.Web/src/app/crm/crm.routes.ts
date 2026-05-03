import { Routes } from "@angular/router";
import { ROUTE_PATHS } from "../shared";
import { AdvertiserListComponent } from "./advertiser/component/advertiser-list/advertiser-list.component";

export const crmRoutes = [
    {
        path: ROUTE_PATHS.ADVERTISER_LIST,
        component: AdvertiserListComponent
    },
    {
        path: '',
        redirectTo: ROUTE_PATHS.ADVERTISER_LIST,
        pathMatch: 'full'
    }
] as Routes;