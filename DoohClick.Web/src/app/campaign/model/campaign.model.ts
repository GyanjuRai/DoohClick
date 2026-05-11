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
