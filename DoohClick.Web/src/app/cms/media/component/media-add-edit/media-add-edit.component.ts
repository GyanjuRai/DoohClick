import {
  Component,
  EventEmitter,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FileService } from '../../../../shared/service/file.service';
import {
  MvFileUploadParam,
  MvFileUploadResult,
} from '../../../../shared/model/file.model';
import { Subject, takeUntil } from 'rxjs';
import { MvMedia } from '../../model/media.model';
import { MvResponse } from '../../../../shared/model/response.model';
import { ResponseStatusEnum } from '../../../../shared';
import { AppConst } from '../../../../app-const';

@Component({
  selector: 'media-add-edit',
  templateUrl: './media-add-edit.component.html',
  styleUrl: './media-add-edit.component.scss',
})
export class MediaAddEditComponent implements OnInit, OnDestroy {
  @Output() afterClose: EventEmitter<MvMedia | null> = new EventEmitter<any>();

  private readonly __unSubscribeAll$: Subject<any>;
  protected isDialogOpen: boolean = false;
  protected displayName: string = '';
  protected file: MvFileUploadResult = {} as MvFileUploadResult;
  protected apiUrl: string = '';

  constructor(private _fileService: FileService) {
    this.__unSubscribeAll$ = new Subject();
    this.apiUrl = AppConst?.data.apiUrl;
  }

  ngOnInit(): void {}

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
        }
      });
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

  protected get isFormValid(): boolean {
    return !!this.file?.fileUrl && !!this.displayName?.trim();
  }

  protected get fileType(): 'video' | 'image' | 'unknown' {
    return this.file?.isVideo ? 'video' : 'image';
  }

  protected close() {
    this.isDialogOpen = false;
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
