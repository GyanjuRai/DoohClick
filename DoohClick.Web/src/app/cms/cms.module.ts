import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MediaAddEditComponent } from './media/component/media-add-edit/media-add-edit.component';
import { MediaListComponent } from './media/component/media-list/media-list.component';
import { RouterModule } from '@angular/router';
import { crmRoutes } from './crm.routes';
import { SharedUiModule } from '../shared/module/shared-ui.module';



@NgModule({
  declarations: [
    MediaAddEditComponent,
    MediaListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(crmRoutes),
    SharedUiModule,
  ]
})
export class CmsModule { }
