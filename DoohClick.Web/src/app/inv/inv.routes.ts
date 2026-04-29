import { Routes } from "@angular/router";
import { ROUTE_PATHS } from "../shared";
import { ScreenListComponent } from "./screen/component/screen-list/screen-list.component";

export const invRoutes = [
    {
        path: ROUTE_PATHS.SCREEN_LIST,
        component: ScreenListComponent
    },
    {
        path: '',
        redirectTo: ROUTE_PATHS.SCREEN_LIST,
        pathMatch: 'full'
    }
] as Routes;