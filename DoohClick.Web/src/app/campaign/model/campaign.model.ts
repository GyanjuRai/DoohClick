export interface MvCampaignFlight {
  id?: number;
  campaignId?: number;
  startDate: Date;
  endDate: Date;
  isDeleted?: boolean;
  screens?: MvCampaignFlightScreen[];
}

export interface MvCampaignFlightScreen {
  id?: number;
  campaignFlightId?: number;
  screenId: number;
}

export interface MvCampaign {
  id?: number;
  campaignCode?: string;
  advertiserId: number;
  advertiser?: string;
  name: string;
  status?: string;
  startDate: Date;
  endDate: Date;
  durationInDays?: number;
  remarks?: string;
  createdAt?: string;
  creator?: string;
  isLocked?: boolean;
  campaignFlight?: MvCampaignFlight[];
}

export interface MvCampaignFilterOptionParam {
  tenantId: number;
  status: string;
  advertiserIdList?: number[];
  startDate?: Date;
  endDate?: Date;
}

export interface MvCampaignIdParam {
  id: number;
  deletedBy?: number;
}

export interface MvCampaignScreenSchedule {
  flightId?: number;
  startDate: string;
  endDate: string;
  screenId?: number;
  screenName?: string;
  screenResolution?: string;
  screenCountry?: string;
  screenCity?: string;
  campaignScreenSchedules: MvScreenSchedule[];
}

export interface MvScreenSchedule {
  scheduleId?: number;
  startTime: string;
  endTime: string;
  dayOfWeek?: string;
  playlist: MvPlaylistItem[];
}

export interface MvPlaylistItem {
  playlistItemId?: number;
  durationSeconds: number;
  playOrder: number;
  displayName?: string;
  fileUrl?: string;
  fileSizeBytes: number;
}

export interface MvCampaignScreenScheduleParam {
  id?: number;
  campaignFlightScreenId: number;
  dayOfWeek: string;
  startTime: string;
  endTime: string;
  createdBy?: number;
  deletedBy?: number;
  playlistItem: MvPlaylistItemParam[];
}

export interface MvPlaylistItemParam {
  id?: number;
  mediaId: number;
  playOrder: number;
  durationSeconds: number;
}
