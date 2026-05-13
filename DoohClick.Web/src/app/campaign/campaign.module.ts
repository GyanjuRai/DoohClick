import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DraftListComponent } from './component/draft/draft-list/draft-list.component';
import { DraftAddEditComponent } from './component/draft/draft-add-edit/draft-add-edit.component';
import { ScheduledListComponent } from './component/scheduled/scheduled-list/scheduled-list.component';
import { CompletedListComponent } from './component/completed/completed-list/completed-list.component';
import { CancelledListComponent } from './component/cancelled/cancelled-list/cancelled-list.component';
import { ActiveListComponent } from './component/active/active-list/active-list.component';
import { CampaignScreenListComponent } from './component/campaign-screen/campaign-screen-list/campaign-screen-list.component';
import { CampaignScreenAddEditComponent } from './component/campaign-screen/campaign-screen-add-edit/campaign-screen-add-edit.component';
import { RouterModule } from '@angular/router';
import { campaignRoutes } from './campaign.routes';
import { SharedUiModule } from '../shared/module/shared-ui.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    DraftListComponent,
    DraftAddEditComponent,
    ScheduledListComponent,
    CompletedListComponent,
    CancelledListComponent,
    ActiveListComponent,
    CampaignScreenListComponent,
    CampaignScreenAddEditComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(campaignRoutes),
    SharedUiModule,
    FormsModule,
    ReactiveFormsModule,
  ],
})
export class CampaignModule {}
