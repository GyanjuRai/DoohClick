import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScreenListComponent } from './screen/component/screen-list/screen-list.component';
import { RouterModule } from '@angular/router';
import { invRoutes } from './inv.routes';
import { ScreenAddEditComponent } from './screen/component/screen-add-edit/screen-add-edit.component';
import { SharedUiModule } from "../shared/module/shared-ui.module";



@NgModule({
  declarations: [
    ScreenListComponent,
    ScreenAddEditComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(invRoutes),
    SharedUiModule
]
})
export class InvModule { }
