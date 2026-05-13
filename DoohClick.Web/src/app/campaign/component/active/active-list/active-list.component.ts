import { Component, OnInit } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';
import { MvCampaign } from '../../../model/campaign.model';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-active-list',
  templateUrl: './active-list.component.html',
  styleUrl: './active-list.component.scss',
})
export class ActiveListComponent extends CampaignBaseClass implements OnInit {
  protected actionMenuItems: MenuItem[] = [];
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.loadCampaing('ACTIVE');
    this.buildBreadcrumb([
      {
        label: 'Active',
        routerLink: `/${this.routes.CAMPAIGN}/${this.routes.CAMPAIGN_ACTIVE}`,
      },
    ]);
  }

  openActionMenu(event: Event, menu: any, campaign: MvCampaign) {
    this.actionMenuItems = [
      {
        label: 'Options',
        items: [
          {
            label: 'Cancel',
            icon: 'pi pi-ban',
            iconClass: 'text-yellow-500',
            command: () => this.onCancel(campaign),
          },
        ],
      },
    ];
    menu.toggle(event);
  }

  onView(campaign: MvCampaign) {}

  onCancel(campaign: MvCampaign) {}
}
