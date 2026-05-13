import {
  Component,
  EventEmitter,
  Injector,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import {
  MvCampaignScreenSchedule,
  MvCampaignScreenScheduleParam,
  MvPlaylistItem,
  MvScreenSchedule,
} from '../../../model/campaign.model';
import {
  MvListitemDdl,
  MvResponse,
} from '../../../../shared/model/response.model';
import { MvListitemDdlParam } from '../../../../shared/model/param.model';
import { Subject, takeUntil } from 'rxjs';
import { ResponseStatusEnum } from '../../../../shared';
import {
  MvMediaDdl,
  MvMediaDdlParam,
} from '../../../../cms/media/model/media.model';
import { MediaService } from '../../../../cms/media/service/media.service';
import { AppConst } from '../../../../app-const';
import { AppComponent } from '../../../../app.component';
import { CampaignService } from '../../../service/campaign.service';

@Component({
  selector: 'campaign-screen-add-edit',
  templateUrl: './campaign-screen-add-edit.component.html',
  styleUrl: './campaign-screen-add-edit.component.scss',
})
export class CampaignScreenAddEditComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  @Output() afterClose: EventEmitter<MvCampaignScreenSchedule | null> =
    new EventEmitter();
  private __unSubscribeAll$: Subject<any> = new Subject();
  private advertiserId!: number;
  private userId: number;

  protected isDialogOpen: boolean = false;
  protected dayList: MvListitemDdl[] = [];
  protected mediaDdl: MvMediaDdl[] = [];
  protected campaignScreen: MvCampaignScreenSchedule =
    {} as MvCampaignScreenSchedule;
  protected scheduleList: MvScreenSchedule[] = [];
  protected expandedScheduleId: number | null = null;

  protected selectedDayOfWeek: string = '';
  protected selectedStartTime: Date | null = null;
  protected selectedEndTime: Date | null = null;
  protected selectedMediaIds: number[] = [];
  protected apiUrl: string = '';

  constructor(
    private injector: Injector,
    private _mediaService: MediaService,
    private _campaignService: CampaignService,
  ) {
    super(injector);
    this.apiUrl = AppConst?.data.apiUrl;
    this.userId = this.auth.getUserId();
  }

  ngOnInit(): void {
    this.getDayList();
  }

  public open(
    campaignScreen: MvCampaignScreenSchedule,
    advertiserId: number,
  ): void {
    this.campaignScreen = campaignScreen;
    this.advertiserId = advertiserId;
    this.scheduleList = [...campaignScreen.campaignScreenSchedules];
    this.expandedScheduleId = null;
    this.resetForm();
    this.getMediaDdl();
    this.isDialogOpen = true;
  }

  protected getDayList(): void {
    const param = { categoryCode: 'DAY_OF_WEEK' } as MvListitemDdlParam;
    this._listItemService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvListitemDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.dayList = [...response.data];
        }
      });
  }

  protected getMediaDdl(): void {
    const param = { advertiserId: this.advertiserId } as MvMediaDdlParam;
    this._mediaService
      .getDdl(param)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe((response: MvResponse<MvMediaDdl[]>) => {
        if (response.type === ResponseStatusEnum.success && response.data) {
          this.mediaDdl = [...response.data];
        }
      });
  }

  protected onAddSchedule(): void {
    if (
      !this.selectedDayOfWeek ||
      !this.selectedStartTime ||
      !this.selectedEndTime
    ) {
      this.showToast(
        'warn',
        'Validation',
        'Please fill day, start time and end time.',
      );
      return;
    }

    const newStart = this.formatTime(this.selectedStartTime);
    const newEnd = this.formatTime(this.selectedEndTime);

    const overlap = this.scheduleList.some((s) => {
      if (s.dayOfWeek !== this.selectedDayOfWeek) return false;
      return newStart < s.endTime && newEnd > s.startTime;
    });

    if (overlap) {
      this.showToast(
        'error',
        'Overlap',
        'This time slot overlaps with an existing schedule.',
      );
      return;
    }

    const playlist: MvPlaylistItem[] = this.selectedMediaIds.map(
      (id, index) => {
        const media = this.mediaDdl.find((m) => m.id === id)!;
        return {
          mediaId: id,
          displayName: media.displayName,
          fileUrl: media.fileUrl,
          durationSeconds: 30,
          playOrder: index + 1,
          fileSizeBytes: media.fileSizeBytes!,
        };
      },
    );

    this.scheduleList.push({
      dayOfWeek: this.selectedDayOfWeek,
      startTime: this.formatTime(this.selectedStartTime),
      endTime: this.formatTime(this.selectedEndTime),
      createdBy: this.userId,
      playlist,
    });

    this.resetForm();
  }

  protected get visibleSchedules(): MvScreenSchedule[] {
    return this.scheduleList.filter((s) => !s.deletedBy);
  }

  protected toggleSchedule(tempId: number): void {
    this.expandedScheduleId =
      this.expandedScheduleId === tempId ? null : tempId;
  }

  protected removeSchedule(
    screenSchedule: MvScreenSchedule,
    event: Event,
  ): void {
    event.stopPropagation();
    const schedule = this.scheduleList.find((s) => s.id === screenSchedule.id);
    if (schedule) schedule.deletedBy = this.userId;

    if (this.expandedScheduleId === screenSchedule.id)
      this.expandedScheduleId = null;
  }

  // protected removePlaylistItem(schedule: ScheduleEntry, mediaId: number): void {
  //   schedule.playlist = schedule.playlist
  //     .filter((p) => p.mediaId !== mediaId)
  //     .map((p, i) => ({ ...p, playOrder: i + 1 }));
  // }

  protected moveUp(schedule: MvScreenSchedule, index: number): void {
    if (index === 0) return;
    const playlist = [...schedule.playlist];
    [playlist[index - 1], playlist[index]] = [
      playlist[index],
      playlist[index - 1],
    ];
    schedule.playlist = playlist.map((p, i) => ({ ...p, playOrder: i + 1 }));
  }

  protected moveDown(schedule: MvScreenSchedule, index: number): void {
    if (index === schedule.playlist.length - 1) return;
    const playlist = [...schedule.playlist];
    [playlist[index + 1], playlist[index]] = [
      playlist[index],
      playlist[index + 1],
    ];
    schedule.playlist = playlist.map((p, i) => ({ ...p, playOrder: i + 1 }));
  }

  protected _afterClose(action: string): void {
    if (action === 'cancel') {
      this.isDialogOpen = false;
      return;
    }

    if (this.scheduleList.length === 0) {
      this.showToast('warn', 'Validation', 'Please add at least one schedule.');
      return;
    }

    const params: MvCampaignScreenScheduleParam[] = this.scheduleList.map(
      (s) => ({
        id: s.id,
        campaignFlightScreenId: this.campaignScreen.campaignFlightScreenId,
        dayOfWeek: s.dayOfWeek!,
        startTime: s.startTime,
        endTime: s.endTime,
        createdBy: s.createdBy,
        deletedBy: s.deletedBy,
        playlistItem: s.playlist.map((p) => ({
          mediaId: p.mediaId,
          playOrder: p.playOrder,
          durationSeconds: p.durationSeconds,
        })),
      }),
    );

    this._campaignService
      .saveSchedule(params)
      .pipe(takeUntil(this.__unSubscribeAll$))
      .subscribe({
        next: (response: MvResponse<MvCampaignScreenSchedule>) => {
          if (response.type === ResponseStatusEnum.success && response.data) {
            this.showToast(
              'success',
              'Success',
              'Schedule saved successfully.',
            );
            this.close(response.data);
          }
        },
        error: () => {
          this.showToast(
            'error',
            'Error',
            'Failed to save schedule. Please try again.',
          );
        },
      });
  }

  protected isImage(fileUrl: string): boolean {
    return /\.(jpg|jpeg|png|webp)$/i.test(fileUrl);
  }

  private resetForm(): void {
    this.selectedDayOfWeek = '';
    this.selectedStartTime = null;
    this.selectedEndTime = null;
    this.selectedMediaIds = [];
  }

  private formatTime(date: Date): string {
    return date.toTimeString().slice(0, 8);
  }

  private close(campaingSchedule: MvCampaignScreenSchedule | null = null) {
    this.afterClose.emit(campaingSchedule);
    this.isDialogOpen = false;
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
