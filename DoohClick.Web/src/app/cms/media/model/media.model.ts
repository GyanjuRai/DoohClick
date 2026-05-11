
export interface MvMedia {
  id?: number;
  tenantId: number;
  advertiserId?: number;
  displayName: string;
  fileName?: string;
  fileUrl?: string;
  fileSizeBytes?: number;
  resolution?: string;
  status?: string;
  durationSec?: number;
  isVideo?: boolean;
  uploadedBy?: number;
  uploadedAt: Date;
  createdBy?: number;
  createdAt: Date;
  creator?: string;
  uploader?: string;
}

export interface MvMediaDdl {
  id?: number;
  displayName: string;
  fileUrl?: string;
  fileSizeBytes?: number;
}

export interface MvMediaFilterOptions {
  tenantId: number;
  isArchieved?: boolean;
  isVideo?: boolean;
}

export interface MvMediaDel {
  id: number;
  tenantId: number;
  deletedBy: number;
}