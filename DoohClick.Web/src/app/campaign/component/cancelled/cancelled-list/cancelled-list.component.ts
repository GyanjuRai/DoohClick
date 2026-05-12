import { Component, OnInit } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';
import { MvCampaign } from '../../../model/campaign.model';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-cancelled-list',
  templateUrl: './cancelled-list.component.html',
  styleUrl: './cancelled-list.component.scss',
})
export class CancelledListComponent
  extends CampaignBaseClass
  implements OnInit
{
  protected actionMenuItems: MenuItem[] = [];

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.loadCampaing('CANCELLED');
    this.buildBreadcrumb([
      {
        label: 'Cancelled',
        routerLink: `/${this.routes.CAMPAIGN}/${this.routes.CAMPAIGN_CANCELLED}`,
      },
    ]);
  }

  openActionMenu(event: Event, menu: any, campaign: MvCampaign) {
    this.actionMenuItems = [
      {
        label: 'Options',
        items: [
          {
            label: 'View',
            icon: 'pi pi-eye',
            iconClass: 'text-blue-500',
            command: () => this.onView(campaign),
          },
          {
            label: 'Resume',
            icon: 'pi pi-play',
            iconClass: 'text-green-500',
            command: () => this.onResume(campaign),
          },
        ],
      },
    ];
    menu.toggle(event);
  }

  onView(campaign: MvCampaign) {}

  onResume(campaign: MvCampaign) {}
}
