import { inject } from "@angular/core";
import { MainLayoutService } from "../service/main-layout.service";
import { ROUTE_PATHS } from "../../../shared/routing/route-paths.const";
import { AuthService } from "../../../core/service/auth.service";

export class BaseComponent
{
    constructor() {}
    protected routes = ROUTE_PATHS;
    protected _mainLayoutService : MainLayoutService = inject(MainLayoutService);
    protected _auth: AuthService = inject(AuthService);
}