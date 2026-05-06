import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { mediaColumn } from '../../model/media-column';
import { finalize, Subject, takeUntil } from 'rxjs';
import { AppComponent } from '../../../../app.component';
import { MediaService } from '../../service/media.service';
import {
  MvGridResponse,
  MvResponse,
} from '../../../../shared/model/response.model';
import { MvMedia, MvMediaDel } from '../../model/media.model';
import { ResponseStatusEnum } from '../../../../shared';
import { AppConst } from '../../../../app-const';
import { MenuItem } from 'primeng/api';
import { mediaMenuItem } from '../../model/media-list-action-items';
import { ConfirmationOptions } from '../../../../shared/model/confirmation.model';

@Component({
  selector: 'media-list',
  templateUrl: './media-list.component.html',
  styleUrl: './media-list.component.scss',
})
export class MediaListComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  private __unSubscribeAll: Subject<any>;
  gridConfig: GridConfig = {
    column: mediaColumn,
    dataSource: {
      data: [],
      totalRows: 0,
    },
    options: {
      filter: {
        tenantId: this.auth.getTenantId(),
        isVideo: null,
        isArchieved: null,
      },
      offset: 0,
      pageSize: 10,
      searchText: '',
    },
  };
  protected isTableLoading: boolean = false;
  protected videoDialogVisible: boolean = false;
  protected isArchived: boolean = false;
  protected actionMenuItems: MenuItem[] = [];
  protected selectedVideoUrl: string | null = null;
  protected mediaTypeList: boolean[] = [];
  protected apiUrl: string = '';

  constructor(
    private injector: Injector,
    private _mediaService: MediaService,
  ) {
    super(injector);
    this.__unSubscribeAll = new Subject();
    this.apiUrl = AppConst?.data.apiUrl;
  }

  ngOnInit(): void {
    this.loadMedia();
  }

  loadMedia() {
    this.isTableLoading = true;
    const param = this.gridConfig.options;

    this._mediaService
      .getGrid(param)
      .pipe(
        takeUntil(this.__unSubscribeAll),
        finalize(() => {
          this.isTableLoading = false;
        }),
      )
      .subscribe((response: MvResponse<MvGridResponse<MvMedia>>) => {
        if (
          response.type === ResponseStatusEnum.success &&
          response.data?.data
        ) {
          this.gridConfig.dataSource.data = [...response.data.data];
          this.gridConfig.dataSource.totalRows = response.data.totalRows;
        }
      });
  }

  openActionMenu(event: Event, menu: any, media: MvMedia) {
    this.actionMenuItems = mediaMenuItem(
      (m) => this.onArchive(m),
      (m) => this.onUnarchive(m),
      media,
    );
    menu.toggle(event);
  }

  afterDialogClose(media: MvMedia | null) {
    if (media) {
      this.gridConfig.dataSource.data = [
        media,
        ...this.gridConfig.dataSource.data,
      ];
      this.gridConfig.dataSource.totalRows++;
      this.showToast(
        'success',
        'Media saved',
        `Media ${media.displayName} save sucessfully!`,
      );
    }
  }

  onArchive(media: MvMedia) {
    const confirmationOptions = {
      message: `Are you sure you want to archive <b>${media.displayName}</b>? It will no longer be available for scheduling.`,
      header: 'Archive Media',
      icon: 'pi pi-inbox',
      acceptButtonStyleClass: 'p-button-warning',
      onAccept: () => {
        const param = {
          id: media.id,
          tenantId: media.tenantId,
          deletedBy: this.auth.getUserId(),
        } as MvMediaDel;

        this._mediaService
          .remove(param)
          .pipe(takeUntil(this.__unSubscribeAll))
          .subscribe((response: MvResponse<MvMedia>) => {
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
                  `${media.displayName} has been archived.`,
                );
              }
            }
          });
      },
    } as ConfirmationOptions;
    this.openConfirmationBox(confirmationOptions);
  }

  onUnarchive(media: MvMedia) {
    const confirmationOptions = {
      message: `Restore <b>${media.displayName}</b> and make it available for scheduling again?`,
      header: 'Unarchive Media',
      icon: 'pi pi-inbox',
      acceptButtonStyleClass: 'p-button-success',
      onAccept: () => {
        this.showToast(
          'info',
          'Coming Soon',
          `Restore feature is not yet available.`,
        );
      },
    } as ConfirmationOptions;
    this.openConfirmationBox(confirmationOptions);
  }

  protected onSearchText() {
    this.gridConfig.options.offset = 0;
    this.loadMedia();
  }

  protected onMediaTypeChange() {
    this.gridConfig.options.filter.isVideo = this.isVideoParam;
    this.loadMedia();
  }

  protected onArchiveFilterChange() {
    this.gridConfig.options.filter.isArchieved = this.isArchived || null;
    this.loadMedia();
  }

  protected openVideoPreview(media: MvMedia): void {
    this.selectedVideoUrl = this.apiUrl + media.fileUrl;
    this.videoDialogVisible = true;
  }

  protected closeVideoPreview(): void {
    this.selectedVideoUrl = null;
    this.videoDialogVisible = false;
  }

  protected onRefresh() {
    this.gridConfig.options.offset = 0;
    this.loadMedia();
  }

  private get isVideoParam(): boolean | null {
    if (!this.mediaTypeList || this.mediaTypeList.length === 0) return null;
    if (this.mediaTypeList.length === 2) return null;
    return this.mediaTypeList[0];
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll.next(null);
    this.__unSubscribeAll.complete();
  }
}
