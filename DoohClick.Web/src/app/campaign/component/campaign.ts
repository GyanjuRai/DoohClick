import { Directive, inject, OnDestroy } from '@angular/core';
import { GridConfig } from '../../shared/model/grid-config.model';
import { campaignColumns } from '../model/campaingn-column';
import { CampaignService } from '../service/campaign.service';
import { MvGridParamOption } from '../../shared/model/param.model';
import {
  MvCampaign,
  MvCampaignFilterOptionParam,
} from '../model/campaign.model';
import { finalize, Subject, takeUntil } from 'rxjs';
import { MvGridResponse, MvResponse } from '../../shared/model/response.model';
import { ResponseStatusEnum, ROUTE_PATHS } from '../../shared';
import { AuthService } from '../../core/service/auth.service';
import { ConfirmationService, MenuItem, MessageService } from 'primeng/api';
import { ConfirmationOptions } from '../../shared/model/confirmation.model';

@Directive({
  selector: 'campaing-base',
})
export abstract class CampaignBaseClass implements OnDestroy {
  private __unSubscribeAll$: Subject<any>;
  private _authSerivce: AuthService;
  private _campaignService: CampaignService;
  private _confirmationService: ConfirmationService;
  private _messageService: MessageService;
  protected gridConfig: GridConfig = {
    column: campaignColumns,
    dataSource: {
      data: [],
      totalRows: 0,
    },
    options: {
      filter: {
        status: 'DRAFT',
        advertiserIdList: [],
      },
      offset: 0,
      pageSize: 10,
      searchText: '',
    },
  };
  protected breadcrumbItems: MenuItem[] = [];
  protected routes = ROUTE_PATHS;
  protected isTableLoading: boolean = false;

  constructor() {
    this.__unSubscribeAll$ = new Subject<any>();
    this._authSerivce = inject(AuthService);
    this._campaignService = inject(CampaignService);
    this._confirmationService = inject(ConfirmationService);
    this._messageService = inject(MessageService);
  }

  loadCampaing(
    status: string = 'DRAFT',
    endDate?: string,
    startDate?: string,
    advertiserIdList?: number[],
  ) {
    this.isTableLoading = true;

    this.gridConfig.options.filter.Status = status;
    this.gridConfig.options.filter.tenantId = this._authSerivce.getTenantId();
    this.gridConfig.options.filter.endDate = endDate;
    this.gridConfig.options.filter.startDate = startDate;
    this.gridConfig.options.filter.advertiserIdList = advertiserIdList;

    const param = {
      filter: this.gridConfig.options.filter,
      offset: this.gridConfig.options.offset,
      pageSize: this.gridConfig.options.pageSize,
      searchText: this.gridConfig.options.searchText,
    } as MvGridParamOption<MvCampaignFilterOptionParam>;

    this._campaignService
      .getGrid(param)
      .pipe(
        takeUntil(this.__unSubscribeAll$),
        finalize(() => {
          this.isTableLoading = false;
        }),
      )
      .subscribe((response: MvResponse<MvGridResponse<MvCampaign>>) => {
        if (
          response.type === ResponseStatusEnum.success &&
          response.data?.data
        ) {
          this.gridConfig.dataSource.data = [...response.data.data ];
          this.gridConfig.dataSource.totalRows = response.data.totalRows;
        }
      });
  }

  protected buildBreadcrumb(labels: MenuItem[]) {
    this.breadcrumbItems = [
      { label: 'Campaign', routerLink: '/campaigns' },
      ...labels,
    ];
  }

  protected onSearchText() {
    this.gridConfig.options.offset = 0;
    this.loadCampaing();
  }

  protected onRefresh() {
    this.gridConfig.options.offset = 0;
    this.loadCampaing();
  }

  /**
   * Displays a toast notification to the user.
   * @param severity visual theme of the toast.
   * @param summary (default) Success. short, bold title for notification.
   * @param details descriptive body text providing more context.
   * @param life (Optional) duration in millisecond before the toast auto-dismissed.
   */
  protected showToast(
    severity: 'success' | 'info' | 'warn' | 'error',
    summary: string = 'Success',
    details: string,
    life: number = 3000,
  ) {
    this._messageService.add({
      severity: severity,
      summary: summary,
      detail: details,
      life: life,
      styleClass: 'toast-lg',
    });
  }

  /**
   * Triggers a global confirmation dialog with the provided configuration.
   * @param confirmationOptions object containing labels, icons, and callback actions.
   */
  protected openConfirmationBox(confirmationOptions: ConfirmationOptions) {
    this._confirmationService.confirm({
      message: confirmationOptions.message,
      header: confirmationOptions.header,
      icon: confirmationOptions?.icon ?? 'pi pi-exclamation-triangle',
      acceptButtonStyleClass:
        confirmationOptions?.acceptButtonStyleClass ?? 'p-button-danger',
      closeOnEscape: confirmationOptions?.closeOnEscape ?? false,
      accept: confirmationOptions.onAccept,
      reject: confirmationOptions.onReject,
    });
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
