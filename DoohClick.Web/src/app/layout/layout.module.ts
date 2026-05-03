import { NgModule } from "@angular/core";
import { MainLayoutComponent } from "./main-layout/component/main-layout.component";
import { AsyncPipe, CommonModule } from "@angular/common";
import { TopbarComponent } from "./main-layout/component/topbar/topbar.component";
import { SidebarComponent } from "./main-layout/component/sidebar/sidebar.component";
import { SharedUiModule } from "../shared/module/shared-ui.module";
import { RouterModule } from "@angular/router";
import { layoutRoutes } from "./layout.route";

@NgModule({
    declarations: [
        MainLayoutComponent,
        TopbarComponent,
        SidebarComponent,
    ],
    imports: [
        CommonModule,
        AsyncPipe,
        SharedUiModule,
        RouterModule.forChild(layoutRoutes),
    ],
    providers: [],
    exports: []
})
export class LayoutModule {}