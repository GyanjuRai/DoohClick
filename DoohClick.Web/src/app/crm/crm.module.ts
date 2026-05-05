import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdvertiserListComponent } from './advertiser/component/advertiser-list/advertiser-list.component';
import { AdvertiserAddEditComponent } from './advertiser/component/advertiser-add-edit/advertiser-add-edit.component';
import { RouterModule } from '@angular/router';
import { crmRoutes } from './crm.routes';
import { SharedUiModule } from '../shared/module/shared-ui.module';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";



@NgModule({
  declarations: [
    AdvertiserListComponent,
    AdvertiserAddEditComponent
  ],
  imports: [
    CommonModule,
    SharedUiModule,
    RouterModule.forChild(crmRoutes),
    FormsModule,
]
})
export class CrmModule { }
