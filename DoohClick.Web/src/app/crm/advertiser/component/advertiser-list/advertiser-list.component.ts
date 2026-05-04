import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { advertiserColumn } from '../../model/advertiser-column';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { finalize, Subject, takeUntil } from 'rxjs';
import { AdvertiserService } from '../../service/advertiser.service';
import { AppComponent } from '../../../../app.component';
import {
  MvGridResponse,
  MvResponse,
} from '../../../../shared/model/response.model';
import { MvAdvertiser } from '../../model/advertiser.model';
import { ResponseStatusEnum } from '../../../../shared';

@Component({
  selector: 'advertiser-list',
  templateUrl: './advertiser-list.component.html',
  styleUrl: './advertiser-list.component.scss',
})
export class AdvertiserListComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  private __unSubscribeAll: Subject<any>;
  gridConfig: GridConfig = {
    column: advertiserColumn,
    dataSource: {
      data: [],
      totalRows: 0,
    },
    options: {
      filter: {
        tenantId: this.auth.getTenantId(),
        isActiveList: [],
      },
      offset: 0,
      pageSize: 10,
      searchText: '',
    },
  };

  protected isTableLoading: boolean = false;
  protected StatusList = [];

  constructor(
    private injector: Injector,
    private _advertiserService: AdvertiserService,
  ) {
    super(injector);
    this.__unSubscribeAll = new Subject();
  }

  ngOnInit(): void {
    this.loadAdvertiser();
  }

  loadAdvertiser() {
    this.isTableLoading = true;
    const param = this.gridConfig.options;

    this._advertiserService
      .getGrid(param)
      .pipe(
        takeUntil(this.__unSubscribeAll),
        finalize(() => {
          this.isTableLoading = false;
        }),
      )
      .subscribe((response: MvResponse<MvGridResponse<MvAdvertiser>>) => {
        if (
          response.type === ResponseStatusEnum.success &&
          response.data?.data
        ) {
          {
            this.gridConfig.dataSource.data = [...response.data.data];
            this.gridConfig.dataSource.totalRows = response.data.totalRows;
          }
        }
        this.isTableLoading = false;
      });
  }

  protected onSearchText() {
    this.gridConfig.options.offset = 0;
    this.loadAdvertiser();
  }

  protected onStatusChange() {
    this.gridConfig.options.offset = 0; 
    this.loadAdvertiser();
  }

  protected onRefresh() {
    this.gridConfig.options.offset = 0; 
    this.loadAdvertiser();
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll.next(null);
    this.__unSubscribeAll.complete();
  }
}
