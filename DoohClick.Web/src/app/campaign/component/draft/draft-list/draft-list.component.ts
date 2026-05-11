import { Component, OnInit, ViewChild } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';
import { MenuItem } from 'primeng/api';
import { MvCampaign } from '../../../model/campaign.model';
import { ConfirmationOptions } from '../../../../shared/model/confirmation.model';
import { DraftAddEditComponent } from '../draft-add-edit/draft-add-edit.component';

@Component({
  selector: 'app-draft-list',
  templateUrl: './draft-list.component.html',
  styleUrl: './draft-list.component.scss',
})
export class DraftListComponent extends CampaignBaseClass implements OnInit {
  @ViewChild('draftAddEdit') draftAddEdit!: DraftAddEditComponent;
  protected actionMenuItems: MenuItem[] = [];

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.loadCampaing();
    this.buildBreadcrumb([
      {
        label: 'Drafts',
        routerLink: `/${this.routes.CAMPAIGN}/${this.routes.CAMPAIGN_DRAFT}`,
      },
    ]);
  }

  openActionMenu(event: Event, menu: any, campaign: MvCampaign) {
    this.actionMenuItems = [
      {
        label: 'Options',
        items: [
          {
            label: 'Edit',
            icon: 'pi pi-pencil',
            iconClass: 'text-orange-500',
            command: () => this.onEdit(campaign),
          },
          {
            label: 'Manage Screens',
            icon: 'pi pi-desktop',
            iconClass: 'text-purple-500',
            command: () => this.onManageScreens(campaign),
          },
          {
            label: 'Approve',
            icon: 'pi pi-check-circle',
            iconClass: 'text-green-500',
            command: () => this.onApprove(campaign),
          },
          {
            label: 'Delete',
            icon: 'pi pi-trash',
            iconClass: 'text-red-500',
            command: () => this.onDelete(campaign),
          },
        ],
      },
    ];

    menu.toggle(event);
  }

  onEdit(campaign: MvCampaign) {
    this.draftAddEdit.open(campaign);
  }

  onManageScreens(campaing: MvCampaign) {}

  onApprove(campaign: MvCampaign) {
    const confirmationOptions = {
      message: `Are you sure you want to approve <b>${campaign.name}</b>?`,
      header: 'Approve Campaign',
      icon: 'pi pi-check-circle',
      acceptButtonStyleClass: 'p-button-success',
      onAccept: () => {},
    } as ConfirmationOptions;
    this.openConfirmationBox(confirmationOptions);
  }

  onDelete(campaign: MvCampaign) {
    const confirmationOptions = {
      message: `Are you sure you want to delete <b>${campaign.name}</b>? .`,
      header: 'Delete Campaign',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      onAccept: () => {},
    } as ConfirmationOptions;
    this.openConfirmationBox(confirmationOptions);
  }
}
