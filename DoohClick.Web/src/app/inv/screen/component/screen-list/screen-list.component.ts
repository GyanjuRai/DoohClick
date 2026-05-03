import {
  Component,
  Injector,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { AppComponent } from '../../../../app.component';
import { finalize, Subject, takeUntil } from 'rxjs';
import { ScreenService } from '../../service/screen.service';
import {
  MvGridResponse,
  MvListitemDdl,
  MvResponse,
} from '../../../../shared/model/response.model';
import { MvScreen, MvScreenFilterOptions } from '../../model/screen.model';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { screenColumn } from '../../model/screen-column';
import { ResponseStatusEnum } from '../../../../shared';
import { MenuItem } from 'primeng/api';
import { screenMenuItem } from '../../model/screen-list-action-items';
import {
  MvGridParamOption,
  MvListitemDdlParam,
} from '../../../../shared/model/param.model';
import { ScreenDetailComponent } from '../screen-detail/screen-detail.component';
import { ScreenAddEditComponent } from '../screen-add-edit/screen-add-edit.component';
import { ConfirmationOptions } from '../../../../shared/model/confirmation.model';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TableLazyLoadEvent } from 'primeng/table';

@Component({
  selector: 'screen-list',
  templateUrl: './screen-list.component.html',
  styleUrl: './screen-list.component.scss',
})
export class ScreenListComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  @ViewChild('screenDetail') screenDetail!: ScreenDetailComponent;
  @ViewChild('screenAddEdit') screenAddEdit!: ScreenAddEditComponent;
  private __unSubscribeAll$: Subject<any>;
  private _tenantId!: number;
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
      searchText: '',
    },
  };
  protected formGroup!: FormGroup;
  protected isTableLoading: boolean = false;
  protected actionMenuItems: MenuItem[] = [];
  protected countryListItemList!: MvListitemDdl[];
  protected cityListItemList!: MvListitemDdl[];
  protected orientationListItemList!: MvListitemDdl[];
  protected resolutionListItemList!: MvListitemDdl[];

  constructor(
    private injector: Injector,
    private _screenService: ScreenService,
    private fb: FormBuilder,
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject<any>();
    this._tenantId = this.auth.getTenantId();
  }

  ngOnInit(): void {
    this.initForm();
    this.loadScreen();
    this.loadDdl();
  }

  initForm() {
    this.formGroup = this.fb.group({
      searchText: [],
      isActive: [],
      countryCodeList: [],
      cityList: [],
      orientationList: [],
      resolutionList: [],
    });
  }

  loadScreen() {
    this.isTableLoading = true;
    const { searchText, ...filterFields } = this.formGroup.value;
    const param: MvGridParamOption<MvScreenFilterOptions> = {
      offset: this.gridConfig.options.offset,
      pageSize: this.gridConfig.options.pageSize,
      searchText: searchText ?? '',
      filter: {
        tenantId: this._tenantId,
        ...Object.fromEntries(
          Object.entries(filterFields).filter(([_, v]) => v !== null),
        ),
      },
    };
    this._screenService
      .getGird(param)
      .pipe(
        takeUntil(this.__unSubscribeAll$),
        finalize(() => {
          this.isTableLoading = false;
        }),
      )
      .subscribe({
        next: (response: MvResponse<MvGridResponse<MvScreen>>) => {
          if (
            response.type === ResponseStatusEnum.success &&
            response.data?.data
          ) {
            this.gridConfig.dataSource.data = [...response.data.data];
            this.gridConfig.dataSource.totalRows = response.data.totalRows;
          }
          this.isTableLoading = false;
        },
        error: () => {
          this.isTableLoading = false;
        },
      });
  }

  loadDdl() {
    this.getCountryDdl();
    this.getCityDdl();
    this.getOrientationDdl();
    this.getResolutionDdl();
  }

  getCountryDdl() {
    const param = {
      categoryCode: 'COUNTRY',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.countryListItemList = [...response.data];
        }
      });
  }

  getCityDdl() {
    const param = {
      categoryCode: 'CITY',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.cityListItemList = [...response.data];
        }
      });
  }

  getOrientationDdl() {
    const param = {
      categoryCode: 'ORIENTATION',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.orientationListItemList = [...response.data];
        }
      });
  }

  getResolutionDdl() {
    const param = {
      categoryCode: 'DEFAULT_RESOLUTION',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.resolutionListItemList = [...response.data];
        }
      });
  }

  addScreen() {
    const screen = {} as MvScreen;
    this.screenAddEdit.openDialog(screen);
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

  onView(screen: MvScreen) {
    this.screenDetail.openDialog(screen);
  }

  onEdit(screen: MvScreen) {
    this.screenAddEdit.openDialog(screen);
  }

  afterFormClose(screen: MvScreen | null) {
    if (screen !== null) {
      const index = this.gridConfig.dataSource.data.findIndex(
        (s) => (s.id = screen.id),
      );
      if (index > -1) {
        this.gridConfig.dataSource.data[index] === screen;
      } else {
        this.gridConfig.dataSource.data.unshift(screen);
        this.gridConfig.dataSource.totalRows++;
      }
      this.gridConfig.dataSource.data = [...this.gridConfig.dataSource.data];
    }
  }

  onDelete(screen: MvScreen) {
    const confirmationOptions = {
      message: `Are you sure you want to delete <b>${screen.name}</b>?`,
      header: 'Confirm Delete',
      icon: 'pi pi-exclamation-triangle',
      acceptButtonStyleClass: 'p-button-danger',
      onAccept: () => {
        this.showToast('success', 'Success', `Product ${screen.name} deleted.`);
      },
    } as ConfirmationOptions;
    this.openConfirmationBox(confirmationOptions);
  }

  applyFilter() {
    this.gridConfig.options.offset = 0;
    this.loadScreen();
  }

  resetFiler() {
    this.formGroup.reset();
  }

  protected onRefresh() {
    this.loadScreen();
    this.resetFiler();
  }

  onPageChange(event: TableLazyLoadEvent) {
    this.gridConfig.options.offset = event.first ?? 0;
    this.gridConfig.options.pageSize = event.rows ?? 10;
    this.loadScreen();
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
