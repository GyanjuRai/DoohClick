import { Component, OnDestroy, OnInit } from '@angular/core';
import { MvCampaignScreenSchedule } from '../../../model/campaign.model';
import {
  MvListitemDdl,
  MvResponse,
} from '../../../../shared/model/response.model';
import { MvListitemDdlParam } from '../../../../shared/model/param.model';
import { ListitemService } from '../../../../shared/service/listitem.service';
import { Subject, takeUntil } from 'rxjs';
import { ResponseStatusEnum } from '../../../../shared';
import {
  MvMediaDdl,
  MvMediaDdlParam,
} from '../../../../cms/media/model/media.model';
import { MediaService } from '../../../../cms/media/service/media.service';
import { AppConst } from '../../../../app-const';

interface ScheduleEntry {
  tempId: number;
  dayOfWeek: string;
  startTime: string;
  endTime: string;
  playlist: PlaylistEntry[];
}

interface PlaylistEntry {
  mediaId: number;
  displayName: string;
  fileUrl?: string;
  durationSeconds: number;
  playOrder: number;
}

@Component({
  selector: 'campaign-screen-add-edit',
  templateUrl: './campaign-screen-add-edit.component.html',
  styleUrl: './campaign-screen-add-edit.component.scss',
})
export class CampaignScreenAddEditComponent implements OnInit, OnDestroy {
  private __unSubscribeAll$: Subject<any> = new Subject();
  private advertiserId!: number;
  private tempIdCounter = 0;

  protected isDialogOpen: boolean = false;
  protected dayList: MvListitemDdl[] = [];
  protected mediaDdl: MvMediaDdl[] = [];
  protected campaignScreen: MvCampaignScreenSchedule =
    {} as MvCampaignScreenSchedule;
  protected scheduleList: ScheduleEntry[] = [];
  protected expandedScheduleId: number | null = null;

  protected selectedDayOfWeek: string = '';
  protected selectedStartTime: Date | null = null;
  protected selectedEndTime: Date | null = null;
  protected selectedMediaIds: number[] = [];
  protected apiUrl: string = '';

  constructor(
    private _listItemService: ListitemService,
    private _mediaService: MediaService,
  ) {
    this.apiUrl = AppConst?.data.apiUrl;
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
    this.scheduleList = [];
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
    )
      return;

    const playlist: PlaylistEntry[] = this.selectedMediaIds.map((id, index) => {
      const media = this.mediaDdl.find((m) => m.id === id)!;
      return {
        mediaId: id,
        displayName: media.displayName,
        fileUrl: media.fileUrl,
        durationSeconds: 30,
        playOrder: index + 1,
      };
    });

    this.scheduleList.push({
      tempId: ++this.tempIdCounter,
      dayOfWeek: this.selectedDayOfWeek,
      startTime: this.formatTime(this.selectedStartTime),
      endTime: this.formatTime(this.selectedEndTime),
      playlist,
    });

    this.resetForm();
  }

  protected toggleSchedule(tempId: number): void {
    this.expandedScheduleId =
      this.expandedScheduleId === tempId ? null : tempId;
  }

  protected removeSchedule(tempId: number, event: Event): void {
    event.stopPropagation();
    this.scheduleList = this.scheduleList.filter((s) => s.tempId !== tempId);
    if (this.expandedScheduleId === tempId) this.expandedScheduleId = null;
  }

  protected removePlaylistItem(schedule: ScheduleEntry, mediaId: number): void {
    schedule.playlist = schedule.playlist
      .filter((p) => p.mediaId !== mediaId)
      .map((p, i) => ({ ...p, playOrder: i + 1 }));
  }

  protected moveUp(schedule: ScheduleEntry, index: number): void {
    if (index === 0) return;
    const playlist = [...schedule.playlist];
    [playlist[index - 1], playlist[index]] = [
      playlist[index],
      playlist[index - 1],
    ];
    schedule.playlist = playlist.map((p, i) => ({ ...p, playOrder: i + 1 }));
  }

  protected moveDown(schedule: ScheduleEntry, index: number): void {
    if (index === schedule.playlist.length - 1) return;
    const playlist = [...schedule.playlist];
    [playlist[index + 1], playlist[index]] = [
      playlist[index],
      playlist[index + 1],
    ];
    schedule.playlist = playlist.map((p, i) => ({ ...p, playOrder: i + 1 }));
  }

  protected _afterClose(action: string): void {
    this.isDialogOpen = false;
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
  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
