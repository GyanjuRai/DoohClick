import { Directive, inject, OnDestroy } from '@angular/core';
import { GridConfig } from '../../shared/model/grid-config.model';
import { campaignColumns } from '../model/campaingn-column';
import { CampaignService } from '../service/campaign.service';
import { MvGridParamOption } from '../../shared/model/param.model';
import {
  MvCampaign,
  MvCampaignFilterOptionParam,
  MvCampaignIdParam,
} from '../model/campaign.model';
import { finalize, Subject, takeUntil } from 'rxjs';
import { MvGridResponse, MvResponse } from '../../shared/model/response.model';
import { ResponseStatusEnum, ROUTE_PATHS } from '../../shared';
import { AuthService } from '../../core/service/auth.service';
import { ConfirmationService, MenuItem, MessageService } from 'primeng/api';
import { ConfirmationOptions } from '../../shared/model/confirmation.model';
import { TableLazyLoadEvent } from 'primeng/table';

@Directive({
  selector: 'campaing-base',
})
export abstract class CampaignBaseClass implements OnDestroy {
  private __unSubscribeAll$: Subject<any>;
  private _authSerivce: AuthService;
  protected _campaignService: CampaignService;
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

  loadCampaing(status: string) {
    this.isTableLoading = true;

    this.gridConfig.options.filter.status = status;
    this.gridConfig.options.filter.tenantId = this._authSerivce.getTenantId();

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
          this.gridConfig.dataSource.data = [...response.data.data];
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

  protected onSearchText(status: string) {
    this.gridConfig.options.offset = 0;
    this.loadCampaing(status);
  }

  protected onRefresh(status: string) {
    this.gridConfig.options.offset = 0;
    this.loadCampaing(status);
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

  protected approveCampaing(campaign: MvCampaign) {
    const param = {
      id: campaign.id,
    } as MvCampaignIdParam;
    this._campaignService
      .approve(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvCampaignIdParam>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          const index = this.gridConfig.dataSource.data.findIndex(
            (m) => m.id === response.data?.id,
          );

          if (index !== -1) {
            this.gridConfig.dataSource.data.splice(index, 1);
            this.gridConfig.dataSource.data = [
              ...this.gridConfig.dataSource.data,
            ]; //refresh grid
            this.gridConfig.dataSource.totalRows--;

            this.showToast(
              'success',
              'Approved',
              `${campaign.campaignCode} has been Approved.`,
            );
          }
        }
      });
  }

  protected removeCampaing(campaign: MvCampaign) {
    const param = {
      id: campaign.id,
    } as MvCampaignIdParam;

    this._campaignService
      .remove(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvCampaignIdParam>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          const index = this.gridConfig.dataSource.data.findIndex(
            (m) => m.id === response.data?.id,
          );

          if (index !== -1) {
            this.gridConfig.dataSource.data.splice(index, 1);
            this.gridConfig.dataSource.data = [
              ...this.gridConfig.dataSource.data,
            ]; //refresh grid
            this.gridConfig.dataSource.totalRows--;

            this.showToast(
              'success',
              'Archived',
              `${campaign.campaignCode} has been archived.`,
            );
          }
        }
      });
  }

  onPageChange(event: TableLazyLoadEvent, status: string) {
    this.gridConfig.options.offset = event.first ?? 0;
    this.gridConfig.options.pageSize = event.rows ?? 10;
    this.loadCampaing(status);
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
