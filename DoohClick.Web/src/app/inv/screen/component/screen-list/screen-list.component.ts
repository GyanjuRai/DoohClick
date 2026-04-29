import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { AppComponent } from '../../../../app.component';
import { finalize, Subject, takeUntil } from 'rxjs';
import { ScreenService } from '../../service/screen.service';
import {
  MvGridResponse,
  MvResponse,
} from '../../../../shared/model/response.model';
import { MvScreen } from '../../model/screen.model';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { screenColumn } from '../../model/screen-column';
import { ResponseStatusEnum } from '../../../../shared';
import { MenuItem } from 'primeng/api';
import { screenMenuItem } from '../../model/screen-list-action-items';

@Component({
  selector: 'screen-list',
  templateUrl: './screen-list.component.html',
  styleUrl: './screen-list.component.scss',
})
export class ScreenListComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  private __unSubscribeAll$: Subject<any>;
  gridConfig: GridConfig = {
    column: screenColumn,
    dataSource: {
      data: [],
      totalRows: 0,
    },
    options: {
      filter: {
        tenantId: 0,
        isActive: true,
        countryCodeList: [],
        cityList: [],
        orientationList: [],
        resolutionList: [],
      },
      offset: 0,
      pageSize: 10,
    },
  };
  protected isTableLoading: boolean = false;
  protected actionMenuItems: MenuItem[] = [];

  constructor(
    private injector: Injector,
    private _screenService: ScreenService,
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject<any>();
  }

  ngOnInit(): void {
    this.loadScreen();
  }

  loadScreen() {
    this.isTableLoading = true;
    const param = this.gridConfig.options;
    param.filter.tenantId = this.auth.getTenantId();
    this._screenService
      .getGird(param)
      .pipe(
        takeUntil(this.__unSubscribeAll$),
        finalize(() => {
          this.isTableLoading = false;
        }),
      )
      .subscribe((response: MvResponse<MvGridResponse<MvScreen>>) => {
        if (
          response.type === ResponseStatusEnum.success &&
          response.data?.data
        ) {
          this.gridConfig.dataSource.data = [...response.data.data];
          this.gridConfig.dataSource.totalRows = response.data.totalRows;
        }
        this.isTableLoading = false;
      });
  }

  openActionMenu(event: Event, menu: any, screen: MvScreen) {
    this.actionMenuItems = screenMenuItem(
      (p) => this.onView(p),
      (p) => this.onEdit(p),
      (p) => this.onDelete(p),
      screen,
    );
    menu.toggle(event);
  }

  onView(screen: MvScreen) {}

  onEdit(screen: MvScreen) {}

  onDelete(screen: MvScreen) {}

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
