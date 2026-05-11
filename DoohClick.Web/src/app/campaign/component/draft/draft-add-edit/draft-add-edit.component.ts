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
import { Subject, takeUntil } from 'rxjs';
import { ScreenService } from '../../../../inv/screen/service/screen.service';
import { AppComponent } from '../../../../app.component';
import { MvResponse } from '../../../../shared/model/response.model';
import { ResponseStatusEnum } from '../../../../shared';
import { AdvertiserService } from '../../../../crm/advertiser/service/advertiser.service';
import { TreeNode } from 'primeng/api';
import { GridColumn } from '../../../../shared/model/grid-config.model';
import { campaignAddEditColumn } from '../../../model/campaing-add-edit.column';

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

  constructor(
    private injector: Injector,
    private _screenService: ScreenService,
    private _advertiserService: AdvertiserService,
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
    this.flightList = campaign?.campaignFlight ?? [];
    this.isDialogOpen = true;
  }

  protected showNodeDetail(event: Event, node: TreeNode) {
    this.hoveredScreen = this.screenDdl.find((s) => s.id === node.data);
  }

  protected addFlight() {}

  protected getScreenNames(screens?: MvCampaignFlightScreen[]): string {
    if (!screens?.length) return '-';
    return screens
      .map((s) => this.screenDdl.find((sc) => sc.id === s.screenId)?.name ?? '')
      .filter(Boolean)
      .join(', ');
  }
  
  protected _afterClose(action: string) {
    this.close();
  }

  private close(screen: MvCampaign | null = null) {
    this.afterClosed.emit(screen);
    this.campaign = {} as MvCampaign;
    this.selectedNodes = [];
    this.flightList = [];
    this.isDialogOpen = false;
    this.screenDdl = [];
    this.advertiserDdl = [];
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
