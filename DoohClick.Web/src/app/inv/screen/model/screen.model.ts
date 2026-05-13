export interface MvScreen {
  id?: number;
  uuid?: string;
  tenantId?: number;
  tenantName?: string;
  name: string;
  normalizedName?: string;
  screenCode: string;
  description?: string;
  defaultResolution: string;
  orientation: string;
  location: string;
  addressLine?: string;
  tag?: string[];
  countryCode: string;
  city: string;
  timezone: string;
  isActive: boolean;
  ratePerHour: number;
  currency: string;
  operatingHour?: MvScreenOperatingHour[];
  supportedMedia?: MvScreenSupportedMedia[];
  createdBy?: number;
  creator?: string;
  updatedBy?: number;
  modifier?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface MvScreenOperatingHour {
  id?: number;
  dayOfWeek: string;
  openTime: string;
  closeTime: string;
  audienceSource: string;
  estimatedImpression?: number;
  deletedBy?: number;
}

export interface MvScreenSupportedMedia {
  id?: number;
  mediaType: string;
  deletedBy?: number;
}

export interface MvScreenDelParam {
  uuid: string;
  tenantId?: number;
  deletedBy?: number;
}

export interface MvScreenFilterOptions {
  tenantId: number;
  isActive?: boolean;
  countryCodeList?: string[];
  cityList?: string[];
  orientationList?: string[];
  resolutionList?: string[];
}

export interface MvScreenDdl {
  id?: number;
  name: string;
  screenCode: string;
  defaultResolution: string;
  orientation: string;
  tag?: string[];
  countryCode: string;
  city: string;
  timezone: string;
  operatingHour: MvScreenOperatingHour[];
  supportedMedia: MvScreenSupportedMedia[];
}
