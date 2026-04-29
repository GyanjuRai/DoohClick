import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScreenListComponent } from './screen/component/screen-list/screen-list.component';
import { RouterModule } from '@angular/router';
import { invRoutes } from './inv.routes';
import { ScreenAddEditComponent } from './screen/component/screen-add-edit/screen-add-edit.component';



@NgModule({
  declarations: [
    ScreenListComponent,
    ScreenAddEditComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(invRoutes),
  ]
})
export class InvModule { }
