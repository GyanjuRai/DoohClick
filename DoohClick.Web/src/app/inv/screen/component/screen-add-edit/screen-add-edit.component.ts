import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  MvScreen,
  MvScreenOperatingHour,
  MvScreenSupportedMedia,
} from '../../model/screen.model';
import { from, Subject, takeUntil } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ScreenService } from '../../service/screen.service';
import { GridColumn } from '../../../../shared/model/grid-config.model';
import { screenAddEditColumn } from '../../model/screen-add-edit.column';
import {
  MvListitemDdl,
  MvResponse,
} from '../../../../shared/model/response.model';
import { AppComponent } from '../../../../app.component';
import { MvListitemDdlParam } from '../../../../shared/model/param.model';
import { ResponseStatusEnum } from '../../../../shared';

@Component({
  selector: 'screen-add-edit',
  templateUrl: './screen-add-edit.component.html',
  styleUrl: './screen-add-edit.component.scss',
})
export class ScreenAddEditComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  @Output() afterClosed: EventEmitter<MvScreen | null> =
    new EventEmitter<MvScreen | null>();

  private __unSubscribeAll$: Subject<any>;
  protected formGroup!: FormGroup;
  protected isDialogOpen: boolean = false;
  protected screen: MvScreen = {} as MvScreen;
  protected columns: GridColumn[] = screenAddEditColumn;
  protected dayList: MvListitemDdl[] = [];
  protected audienceSourceList: MvListitemDdl[] = [];
  protected operatingHourList: MvScreenOperatingHour[] = [];
  protected countryListItemList!: MvListitemDdl[];
  protected cityListItemList!: MvListitemDdl[];
  protected orientationListItemList!: MvListitemDdl[];
  protected resolutionListItemList!: MvListitemDdl[];
  protected timezoneListItemList!: MvListitemDdl[];
  protected screenTagList!: MvListitemDdl[];
  protected currencyListItemList!: MvListitemDdl[];
  protected mediaTypeListItemList!: MvListitemDdl[];
  protected supportedMediaList: MvScreenSupportedMedia[] = [];

  constructor(
    private fb: FormBuilder,
    private _screenService: ScreenService,
    private injector: Injector,
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject<any>();
  }

  ngOnInit(): void {
    this.initForm();
    this.getDayList();
    this.getAudienceSrcList();
    this.getCountryDdl();
    this.getCityDdl();
    this.getOrientationDdl();
    this.getTimeZoneDdl();
    this.getResolutionDdl();
    this.getCurrencyDdl();
    this.getTagDdl();
    this.getMediaTypeDdl();
  }

  protected initForm() {
    this.formGroup = this.fb.group({
      name: [this.screen.name ?? '', Validators.required],
      description: [this.screen.description ?? ''],
      defaultResolution: [
        this.screen?.defaultResolution ?? '',
        Validators.required,
      ],
      orientation: [this.screen?.orientation ?? '', Validators.required],
      timezone: [this.screen?.timezone ?? '', Validators.required],
      countryCode: [this.screen?.countryCode ?? '', Validators.required],
      city: [this.screen?.city ?? '', Validators.required],
      location: [this.screen?.location ?? '', Validators.required],
      addressLine: [this.screen?.addressLine ?? ''],
      tag: [this.screen?.tag ?? []],
      isActive: [this.screen?.isActive ?? true],
      ratePerHour: [this.screen?.ratePerHour ?? null, Validators.required],
      currency: [this.screen?.currency ?? '', Validators.required],

      operatingHour: this.fb.group({
        dayOfWeek: [null],
        openTime: [this.getDefaultTime()],
        closeTime: [this.getDefaultTime()],
        estimatedImpression: [null],
        audienceSource: [null],
      }),
    });
  }

  protected getDayList() {
    const param = {
      categoryCode: 'DAY_OF_WEEK',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.dayList = [...response.data];
        }
      });
  }

  protected getAudienceSrcList() {
    const param = {
      categoryCode: 'AUDIENCE_SOURCE',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.audienceSourceList = [...response.data];
        }
      });
  }

  protected getCountryDdl() {
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

  protected getCityDdl() {
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

  protected getOrientationDdl() {
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

  protected getResolutionDdl() {
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

  protected getTimeZoneDdl() {
    const param = {
      categoryCode: 'TIMEZONE',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.timezoneListItemList = [...response.data];
        }
      });
  }

  protected getTagDdl() {
    const param = {
      categoryCode: 'SCREEN_TAG',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.screenTagList = [...response.data];
        }
      });
  }

  protected getCurrencyDdl() {
    const param = {
      categoryCode: 'CURRENCY',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.currencyListItemList = [...response.data];
        }
      });
  }

  protected getMediaTypeDdl() {
    const param = {
      categoryCode: 'MEDIA_TYPE',
    } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.mediaTypeListItemList = [...response.data];
        }
      });
  }

  protected get dialogHeader(): string {
    return this.screen?.id ? 'Screen details' : 'Create new screen';
  }

  protected get action(): string {
    return this.screen?.id ? 'edit' : 'save';
  }

  public openDialog(screen: MvScreen) {
    if (screen) {
      this.screen = screen;
      this.operatingHourList = screen.operatingHour ?? [];
      this.supportedMediaList = screen.supportedMedia ?? [];
      this.formGroup.reset(this.screen);
    } else {
      this.screen = {} as MvScreen;
      this.operatingHourList = [];
      this.supportedMediaList = [];
      this.formGroup.reset();
    }
    this.isDialogOpen = true;
  }

  protected onDialogShow() {
    this.formGroup.get('operatingHour')?.patchValue({
      dayOfWeek: null,
      openTime: this.getDefaultTime(),
      closeTime: this.getDefaultTime(),
      estimatedImpression: null,
      audienceSource: null,
    });
  }

  protected _afterClose(action: string) {
    if (action === 'cancel') {
      this.close();
      return;
    } else {
      if (this.formGroup.valid && (this.formGroup.dirty || action === 'edit')) {
        let param: MvScreen = this.buildPayload(); 

        this._screenService
          .save(param)
          .pipe(takeUntil(this.__unSubscribeAll$))
          .subscribe({
            next: (response: MvResponse<MvScreen>) => {
              if (
                response.type === ResponseStatusEnum.success &&
                response.data
              ) {
                this.showToast(
                  'success',
                  'Screen Saved',
                  `Screen ${response.data.name} saved!`,
                );
                this.close(response.data);
              }
            },
            error: () => {
              this.showToast(
                'error',
                'Failed to save',
                `Failed to save screen ${param.name}`,
              );
            },
          });
      }
    }
  }

  protected onAddOperatingHour() {
    const oh = this.formGroup.get('operatingHour')?.value;
    this.operatingHourList.push({
      ...oh,
      openTime: this.toTimeString(oh.openTime),
      closeTime: this.toTimeString(oh.closeTime),
      deletedBy: null,
    });
    this.formGroup.get('operatingHour')?.reset({
      dayOfWeek: null,
      openTime: this.getDefaultTime(),
      closeTime: this.getDefaultTime(),
      estimatedImpression: null,
      audienceSource: null,
    });
  }

  protected onRemoveOperatingHour(oh: MvScreenOperatingHour) {
    if (oh.id) {
      oh.deletedBy = this.auth.getUserId();
    } else {
      this.operatingHourList = this.operatingHourList.filter((x) => x !== oh);
    }
  }

  protected get visibleOperatingHour() {
    return this.operatingHourList.filter((x) => x.deletedBy === null);
  }

  protected onRemoveMedia(media: MvScreenSupportedMedia): void {
    if (media.id) {
      media.deletedBy = this.auth.getUserId();
    } else {
      this.supportedMediaList = this.supportedMediaList.filter(
        (x) => x.mediaType !== media.mediaType,
      );
    }
  }

  protected onAddMedia(event: any): void {
    const already = this.supportedMediaList.some(
      (x) => x.mediaType === event.value,
    );
    if (!already) {
      this.supportedMediaList.push({
        id: null as any,
        mediaType: event.value,
        deletedBy: null as any,
      });
    }
  }

  protected get visibleSupportedMedia() {
    return this.supportedMediaList.filter((x) => x.deletedBy === null);
  }

  private buildPayload(): MvScreen {
    const fg = this.formGroup.getRawValue();

    return {
      id: this.screen?.id,
      tenantId: this.screen?.tenantId || this.auth.getTenantId(),
      name: fg.name,
      screenCode: fg.screenCode,
      description: fg.description,
      defaultResolution: fg.defaultResolution,
      orientation: fg.orientation,
      timezone: fg.timezone,
      countryCode: fg.countryCode,
      city: fg.city,
      location: fg.location,
      addressLine: fg.addressLine,
      tag: fg.tag,
      isActive: fg.isActive,
      ratePerHour: fg.ratePerHour,
      currency: fg.currency,
      operatingHour: this.operatingHourList,
      supportedMedia: this.supportedMediaList,
    };
  }

  private getDefaultTime(): Date {
    const d = new Date();
    d.setHours(0, 0, 0, 0);
    return d;
  }

  private toTimeString(date: Date | null): string {
    if (!date) return '00:00:00';
    const h = date.getHours().toString().padStart(2, '0');
    const m = date.getMinutes().toString().padStart(2, '0');
    return `${h}:${m}:00`;
  }

  private close(screen: MvScreen | null = null) {
    this.afterClosed.emit(screen);
    this.screen = {} as MvScreen;
    this.isDialogOpen = false;
    this.operatingHourList = [];
    this.supportedMediaList = [];
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
