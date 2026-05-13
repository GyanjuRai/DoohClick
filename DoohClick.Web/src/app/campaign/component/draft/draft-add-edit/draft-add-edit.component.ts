import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  MvCampaign,
  MvCampaignFlight,
  MvCampaignFlightScreen,
} from '../../../model/campaign.model';
import {
  MvAdvertiserDdl,
  MvTenantIdParam,
} from '../../../../crm/advertiser/model/advertiser.model';
import { MvScreenDdl } from '../../../../inv/screen/model/screen.model';
import { finalize, Subject, takeUntil } from 'rxjs';
import { ScreenService } from '../../../../inv/screen/service/screen.service';
import { AppComponent } from '../../../../app.component';
import { MvResponse } from '../../../../shared/model/response.model';
import { ResponseStatusEnum } from '../../../../shared';
import { AdvertiserService } from '../../../../crm/advertiser/service/advertiser.service';
import { TreeNode } from 'primeng/api';
import { GridColumn } from '../../../../shared/model/grid-config.model';
import { campaignAddEditColumn } from '../../../model/campaing-add-edit.column';
import { CampaignService } from '../../../service/campaign.service';

@Component({
  selector: 'draft-add-edit',
  templateUrl: './draft-add-edit.component.html',
  styleUrl: './draft-add-edit.component.scss',
})
export class DraftAddEditComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  @Output() protected afterClosed: EventEmitter<MvCampaign | null> =
    new EventEmitter<MvCampaign | null>();

  private __unSubscribeAll$: Subject<any>;
  protected _closing: boolean = false;
  protected isDialogOpen: boolean = false;
  protected columns: GridColumn[] = campaignAddEditColumn;
  protected activeIndex: number = 0;
  protected advertiserDdl: MvAdvertiserDdl[] = [];
  protected screenDdl: MvScreenDdl[] = [];
  protected campaign: MvCampaign = {} as MvCampaign;
  protected nodes!: TreeNode[];
  protected selectedNodes: TreeNode[] = [];
  protected hoveredScreen?: MvScreenDdl;
  protected currentFlight: MvCampaignFlight = {} as MvCampaignFlight;
  protected flightList: MvCampaignFlight[] = [];
  protected touched: Record<string, boolean> = {};

  constructor(
    private injector: Injector,
    private _screenService: ScreenService,
    private _advertiserService: AdvertiserService,
    private _campaignService: CampaignService,
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject();
  }

  ngOnInit(): void {
    this.getDdl();
  }

  protected groupByCity(screens: MvScreenDdl[]): TreeNode[] {
    const cityMap = new Map<string, MvScreenDdl[]>();

    screens.forEach((s) => {
      if (!cityMap.has(s.city)) {
        cityMap.set(s.city, []);
      }
      cityMap.get(s.city)!.push(s);
    });

    return Array.from(cityMap.entries()).map(([city, list]) => ({
      label: city,
      selectable: false,
      children: list.map((s) => ({
        label: s.name,
        data: s.id,
        key: String(s.id),
      })),
    }));
  }

  protected getDdl() {
    this.getScreenDdl();
    this.getAdvertiserDdl();
  }

  protected getScreenDdl() {
    const param = {
      tenantId: this.auth.getTenantId(),
    } as MvTenantIdParam;

    this._screenService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvScreenDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.screenDdl = [...response.data];
          this.nodes = this.groupByCity(this.screenDdl);
        }
      });
  }

  protected getAdvertiserDdl() {
    const param = {
      tenantId: this.auth.getTenantId(),
    } as MvTenantIdParam;

    this._advertiserService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvAdvertiserDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.advertiserDdl = [...response.data];
        }
      });
  }

  protected get dialogHeader(): string {
    return this.campaign?.id ? 'Campaign details' : 'Create new campaign';
  }

  protected get action(): string {
    return this.campaign?.id ? 'edit' : 'save';
  }

  public open(campaign?: MvCampaign) {
    this.campaign = campaign ?? ({} as MvCampaign);
    this.flightList = campaign?.campaignFlight
      ? [...campaign.campaignFlight]
      : [];
    this.isDialogOpen = true;
  }

  protected showNodeDetail(event: Event, node: TreeNode) {
    this.hoveredScreen = this.screenDdl.find((s) => s.id === node.data);
  }

  protected markTouched(field: string) {
    this.touched[field] = true;
  }

  protected markAllTouched() {
    ['name', 'advertiserId', 'startDate', 'endDate'].forEach(
      (f) => (this.touched[f] = true),
    );
  }

  protected addFlight() {
    if (!this.isValidFlight()) return;

    let flight = {
      startDate: this.currentFlight.startDate,
      endDate: this.currentFlight.endDate,
      screens: this.selectedNodes
        .filter((n) => n.data != null)
        .map((n) => ({ screenId: n.data as number })),
    } as MvCampaignFlight;

    this.flightList.push(flight);
    this.currentFlight = {} as MvCampaignFlight;
    this.selectedNodes = [];
  }

  protected removeFlight(index: number) {
    this.flightList.splice(index, 1);
  }

  protected getScreenNames(screens?: MvCampaignFlightScreen[]): string {
    if (!screens?.length) return '-';
    return screens
      .map((s) => this.screenDdl.find((sc) => sc.id === s.screenId)?.name ?? '')
      .filter(Boolean)
      .join(', ');
  }

  protected _afterClose(action: string) {
    if (this._closing) {
      return;
    }
    if (action === 'cancel') {
      this._closing = false;
      this.close();
      return;
    }

    this.markAllTouched();
    if (!this.isValid()) {
      this._closing = false;
      return;
    }

    this._closing = true;

    const param = {
      ...this.campaign,
      startDate: this.campaign.startDate,
      endDate: this.campaign.endDate,
      campaignFlight: this.flightList.map((f) => ({
        ...f,
        startDate: f.startDate,
        endDate: f.endDate,
      })),
    } as MvCampaign;

    this._campaignService
      .save(param)
      .pipe(
        takeUntil(this.__unSubscribeAll$),
        finalize(() => {
          this._closing = false;
        }),
      )
      .subscribe({
        next: (response: MvResponse<MvCampaign>) => {
          if (response.type === ResponseStatusEnum.success && response.data) {
            this.showToast(
              'success',
              'Success',
              'Campaign saved successfully.',
            );
            this.close(response.data);
          }
        },
        error: () => {
          this.showToast(
            'error',
            'Error',
            'Failed to save campaign. Please try again.',
          );
        },
      });

    this.close();
  }

  private isValid(): boolean {
    return !!(
      this.campaign.name?.trim() &&
      this.campaign.advertiserId &&
      this.campaign.startDate &&
      this.campaign.endDate
    );
  }

  private isValidFlight(): boolean {
    const start = new Date(this.currentFlight.startDate);
    const end = new Date(this.currentFlight.endDate);

    const overlaps = this.flightList.some((f) => {
      const fStart = new Date(f.startDate);
      const fEnd = new Date(f.endDate);
      return start <= fEnd && end >= fStart;
    });

    if (overlaps) {
      this.showToast(
        'error',
        'Overlap Detected',
        'Flight dates overlap with an existing flight.',
      );
      return false;
    }

    return true;
  }

  protected onDialogHidden() {
    this.activeIndex = 0;
    this.touched = {};

    if (!this._closing) {
      this.close();
    }

    this._closing = false;
  }

  protected toDateOnly(date: string): Date {
    return new Date(date);
  }

  private close(screen: MvCampaign | null = null) {
    this.afterClosed.emit(screen);
    this.campaign = {} as MvCampaign;
    this.selectedNodes = [];
    this.flightList = [];
    this.isDialogOpen = false;
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
