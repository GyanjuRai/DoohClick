import { Routes } from "@angular/router";
import { ROUTE_PATHS } from "../shared";
import { DraftListComponent } from "./component/draft/draft-list/draft-list.component";
import { ScheduledListComponent } from "./component/scheduled/scheduled-list/scheduled-list.component";
import { ActiveListComponent } from "./component/active/active-list/active-list.component";
import { CompletedListComponent } from "./component/completed/completed-list/completed-list.component";
import { CancelledListComponent } from "./component/cancelled/cancelled-list/cancelled-list.component";
import { CampaignScreenListComponent } from "./component/campaign-screen/campaign-screen-list/campaign-screen-list.component";

export const campaignRoutes = [
    {
        path: ROUTE_PATHS.CAMPAIGN_DRAFT,
        component: DraftListComponent
    },
    {
        path: ROUTE_PATHS.CAMPAIGN_SCHEDULED,
        component: ScheduledListComponent
    },
    {
        path: ROUTE_PATHS.CAMPAIGN_ACTIVE,
        component: ActiveListComponent
    },
    {
        path: ROUTE_PATHS.CAMPAIGN_COMPLETED,
        component: CompletedListComponent
    },
    {
        path: ROUTE_PATHS.CAMPAIGN_CANCELLED,
        component: CancelledListComponent
    },
    {
        path: `:id/:advertiserId/:name/${ROUTE_PATHS.CAMPAIGN_MEDIA}/:mode`,
        component: CampaignScreenListComponent
    },
    {
        path: '',
        redirectTo: ROUTE_PATHS.CAMPAIGN_DRAFT,
        pathMatch: 'full'
    }
] as Routes;