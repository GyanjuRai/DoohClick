import { Component, inject, OnInit } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';
import { ActivatedRoute } from '@angular/router';
import {
  MvCampaignIdParam,
  MvCampaignScreenSchedule,
} from '../../../model/campaign.model';
import { finalize, takeUntil } from 'rxjs';
import { MvResponse } from '../../../../shared/model/response.model';
import { ResponseStatusEnum } from '../../../../shared';
import { GridColumn } from '../../../../shared/model/grid-config.model';
import { campaignScreenColumn } from '../../../model/campaign-screen.column';

@Component({
  selector: 'campaign-screen-list',
  templateUrl: './campaign-screen-list.component.html',
  styleUrl: './campaign-screen-list.component.scss',
})
export class CampaignScreenListComponent
  extends CampaignBaseClass
  implements OnInit
{
  private route = inject(ActivatedRoute);
  protected campaignId!: number;
  protected advertiserId!: number;
  protected isLoading: boolean = false;
  protected screens: MvCampaignScreenSchedule[] = [];
  protected columns: GridColumn[] = campaignScreenColumn;
  protected expandedScreenId: number | null = null;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.buildBreadcrumb([
      {
        label: 'Drafts',
        routerLink: `/${this.routes.CAMPAIGN}/${this.routes.CAMPAIGN_DRAFT}`,
      },
      {
        label: 'Campaign Media',
      },
    ]);

    this.campaignId = Number(this.route.snapshot.paramMap.get('id'));
    this.advertiserId = Number(this.route.snapshot.paramMap.get('advertiserId'));
    this.loadSchedule();
  }

  loadSchedule(): void {
    this.isLoading = true;
    const param = { id: this.campaignId } as MvCampaignIdParam;

    this._campaignService
      .getSchedules(param)
      .pipe(
        takeUntil(this.__unSubscribeAll$),
        finalize(() => (this.isLoading = false)),
      )
      .subscribe((response: MvResponse<MvCampaignScreenSchedule[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.screens = response.data;
        }
      });
  }

  protected flightDateLabel(screen: MvCampaignScreenSchedule): string {
    const fmt = (d: string) =>
      new Date(d).toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric',
      });
    return `${fmt(screen.startDate)} – ${fmt(screen.endDate)}`;
  }

  protected toggleExpand(screenId: number): void {
    this.expandedScreenId =
      this.expandedScreenId === screenId ? null : screenId;
  }

  onSchedule(screen: MvCampaignScreenSchedule): void {}
}
