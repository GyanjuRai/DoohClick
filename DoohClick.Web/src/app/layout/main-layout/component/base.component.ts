import { inject } from "@angular/core";
import { MainLayoutService } from "../service/main-layout.service";
import { ROUTE_PATHS } from "../../../shared/routing/route-paths.const";

export class BaseComponent
{
    constructor() {}
    protected routes = ROUTE_PATHS;
    protected _mainLayoutService : MainLayoutService = inject(MainLayoutService);
}