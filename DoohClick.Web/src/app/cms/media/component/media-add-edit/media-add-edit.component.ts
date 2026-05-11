import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FileService } from '../../../../shared/service/file.service';
import {
  MvFileUploadParam,
  MvFileUploadResult,
} from '../../../../shared/model/file.model';
import { finalize, Subject, takeUntil } from 'rxjs';
import { MvMedia } from '../../model/media.model';
import { MvResponse } from '../../../../shared/model/response.model';
import { ResponseStatusEnum } from '../../../../shared';
import { AppConst } from '../../../../app-const';
import { AppComponent } from '../../../../app.component';
import { MediaService } from '../../service/media.service';
import { MvAdvertiserDdl, MvTenantIdParam } from '../../../../crm/advertiser/model/advertiser.model';
import { AdvertiserService } from '../../../../crm/advertiser/service/advertiser.service';

@Component({
  selector: 'media-add-edit',
  templateUrl: './media-add-edit.component.html',
  styleUrl: './media-add-edit.component.scss',
})
export class MediaAddEditComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  @Output() afterClose: EventEmitter<MvMedia | null> = new EventEmitter<any>();

  private readonly __unSubscribeAll$: Subject<any>;
  protected isDialogOpen: boolean = false;
  protected displayName: string = '';
  protected file: MvFileUploadResult = {} as MvFileUploadResult;
  protected apiUrl: string = '';
  protected advertiserDdl: MvAdvertiserDdl[] = [];
  protected selectedAdvertiserId?: number;

  constructor(
    private injector: Injector,
    private _fileService: FileService,
    private _mediaService: MediaService,
    private _advertiserService: AdvertiserService
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject();
    this.apiUrl = AppConst?.data.apiUrl;
  }

  ngOnInit(): void {
    this.getAdvertiserDdl();
  }

  protected getAdvertiserDdl() {

    const param = {
      tenantId: this.auth.getTenantId()
    } as MvTenantIdParam;

    this._advertiserService.getDdl(param)
    .pipe(takeUntil(this.__unSubscribeAll$))
    .subscribe((response: MvResponse<MvAdvertiserDdl[]>) => {
      if (response.type === ResponseStatusEnum.success && response.data) {
        this.advertiserDdl = [...response.data];
      }
    })
  }

  public open() {
    this.isDialogOpen = true;
  }

  protected onUpload(event: any) {
    const formData = new FormData();
    formData.append('File', event.files[0]);

    this._fileService
      .upload(formData)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvFileUploadResult>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.file = response.data;
          this.showToast(
            'success',
            'Uploaded',
            `${response.data.fileName} uploaded !`,
          );
        }
      });
  }

  protected save() {
    const param = {
      displayName: this.displayName,
      ...this.file,
      advertiserId: this.selectedAdvertiserId,
      uploadedBy: this.auth.getUserId(),
    } as MvMedia;

    this._mediaService
      .add(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvMedia>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.close(response.data);
        }
      });
  }

  protected get isFormValid(): boolean {
    return !!this.file?.fileUrl && !!this.displayName?.trim();
  }

  protected get fileType(): 'video' | 'image' | 'unknown' {
    return this.file?.isVideo ? 'video' : 'image';
  }

  protected choose(callback: () => void) {
    callback();
  }

  protected uploadEvent(callback: () => void) {
    callback();
  }

  protected onRemoveTemplatingFile(
    event: any,
    removeFileCallback: (event: any, index: any) => void,
    index: any,
  ) {
    removeFileCallback(event, index);
  }

  protected isVideo(file: File): boolean {
    return file.type.startsWith('video/');
  }

  protected clearUpload() {
    this.file = {} as MvFileUploadResult;
  }

  protected close(media: MvMedia | null = null) {
    this.afterClose.emit(media);
    this.isDialogOpen = false;
    this.file = {} as MvFileUploadResult;
    this.displayName = '';
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
