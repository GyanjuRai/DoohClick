import { Routes } from "@angular/router";
import { ROUTE_PATHS } from "../shared";
import { MediaListComponent } from "./media/component/media-list/media-list.component";

export const crmRoutes = [
    {
        path: ROUTE_PATHS.MEDIA_LIST,
        component: MediaListComponent
    },
    {
        path: '',
        redirectTo: ROUTE_PATHS.MEDIA_LIST,
        pathMatch: 'full'
    }
] as Routes;