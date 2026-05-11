import { Injectable } from '@angular/core';
import { WebApiService } from '../../shared/service/web-api.service';
import { MvGridParamOption } from '../../shared/model/param.model';
import {
  MvCampaign,
  MvCampaignFilterOptionParam,
  MvCampaignIdParam,
} from '../model/campaign.model';
import { map, Observable } from 'rxjs';
import { MvGridResponse, MvResponse } from '../../shared/model/response.model';

@Injectable({
  providedIn: 'root',
})
export class CampaignService {
  constructor(private api: WebApiService) {}

  getGrid(
    param: MvGridParamOption<MvCampaignFilterOptionParam>,
  ): Observable<MvResponse<MvGridResponse<MvCampaign>>> {
    return this.api.get('commercial/campaign/grid', param, true).pipe(
      map((response) => {
        if (response.data?.data) {
          response.data.data = response.data.data.map(this.mapDates);
        }
        return response;
      }),
    );
  }

  save(param: MvCampaign): Observable<MvResponse<MvCampaign>> {
    return this.api.post('commercial/campaign', param);
  }

  remove(param: MvCampaignIdParam): Observable<MvResponse<MvCampaignIdParam>> {
    return this.api.delete('commercial/campaign/{id}', param);
  }

  private mapDates(c: MvCampaign): MvCampaign {
    return {
      ...c,
      startDate: new Date(c.startDate),
      endDate: new Date(c.endDate),
      campaignFlight: c.campaignFlight?.map((f) => ({
        ...f,
        startDate: new Date(f.startDate),
        endDate: new Date(f.endDate),
      })),
    };
  }
}
