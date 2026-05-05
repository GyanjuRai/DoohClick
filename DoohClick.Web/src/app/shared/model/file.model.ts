export interface MvFileUploadParam {
  file: File;
}

export interface MvFileUploadResult {
  fileName: string;
  fileUrl: string;
  resolution?: string;
  durationSec?: number;
  isVideo?: boolean;
  fileSizeBytes?: number;
}