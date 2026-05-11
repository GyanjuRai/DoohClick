import { Injectable } from '@angular/core';
import { WebApiService } from '../../../shared/service/web-api.service';
import { Observable } from 'rxjs';
import {
  MvGridResponse,
  MvResponse,
} from '../../../shared/model/response.model';
import { MvGridParamOption } from '../../../shared/model/param.model';
import { MvScreen, MvScreenDdl, MvScreenFilterOptions } from '../model/screen.model';
import { MvTenantIdParam } from '../../../crm/advertiser/model/advertiser.model';

@Injectable({ providedIn: 'root' })
export class ScreenService {
  constructor(private api: WebApiService) {}

  getGird(
    param: MvGridParamOption<MvScreenFilterOptions>,
  ): Observable<MvResponse<MvGridResponse<MvScreen>>> {
    return this.api.get('inv/screen/grid', param, true);
  }

  save(param: MvScreen): Observable<MvResponse<MvScreen>> {
    return this.api.post('inv/screens', param);
  }

  remove(id: number): Observable<MvResponse<MvScreen>> {
    return this.api.delete(`inv/screen/${id}`);
  }

  getDdl(param: MvTenantIdParam): Observable<MvResponse<MvScreenDdl[]>> {
    return this.api.get('inv/screen/ddl', param);
  }
}
