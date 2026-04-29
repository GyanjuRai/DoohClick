import { Routes } from "@angular/router";
import { LoginComponent } from "./component/login/login.component";
import { ROUTE_PATHS } from "../../shared";

export const authRoutes = [
    {
        path: ROUTE_PATHS.LOGIN,
        component: LoginComponent
    },
    {
        path: '',
        redirectTo: ROUTE_PATHS.LOGIN,
        pathMatch: 'full'
    }
] as Routes;