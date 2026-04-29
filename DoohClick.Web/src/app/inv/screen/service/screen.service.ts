import { Injectable } from '@angular/core';
import { WebApiService } from '../../../shared/service/web-api.service';
import { Observable } from 'rxjs';
import {
  MvGridResponse,
  MvResponse,
} from '../../../shared/model/response.model';
import { MvGridParamOption } from '../../../shared/model/param.model';
import { MvScreen, MvScreenFilterOptions } from '../model/screen.model';

@Injectable({ providedIn: 'root' })
export class ScreenService {
  constructor(private api: WebApiService) {}

  getGird(
    param: MvGridParamOption<MvScreenFilterOptions>,
  ): Observable<MvResponse<MvGridResponse<MvScreen>>> {
    return this.api.get('inv/screen/grid', param);
  }

  save() {
    return this.api.post('', {});
  }

  remove() {
    return this.api.delete('');
  }
}
