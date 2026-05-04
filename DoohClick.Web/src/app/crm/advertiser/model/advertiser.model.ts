export interface MvAdvertiser {
  id: number;
  name: string;
  contactName: string;
  contactEmail: string;
  contactPhone: string;
  isActive: boolean;
  createdBy: number;
  updatedBy: number | null;
  creator: string | null;
  createdAt: string;
  updatedAt: string | null;
}

export interface MvAdvertiserDdl {
  id: number;
  name: string;
}

export interface MvTenantIdParam {
  tenantId: number;
}

export interface MvAdvertiserFilterOptions {
  tenantId: number;
  isActiveList: boolean[] | null;
}
