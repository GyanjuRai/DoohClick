import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { AppComponent } from '../../../../app.component';
import { Subject, takeUntil } from 'rxjs';
import { ScreenService } from '../../service/screen.service';
import { MvGridResponse, MvResponse } from '../../../../shared/model/response.model';
import { MvScreen } from '../../model/screen.model';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { screenColumn } from '../../model/screen-column';

@Component({
  selector: 'app-screen-list',
  templateUrl: './screen-list.component.html',
  styleUrl: './screen-list.component.scss',
})
export class ScreenListComponent
  extends AppComponent
  implements OnInit, OnDestroy
{
  private __unSubscribeAll$: Subject<any>;
  gridConfig: GridConfig = {
    column: screenColumn,
    dataSource: {
      data: [],
      totalRows: 0
    },
    options: {
      offset: 0,
      pageSize: 10,
    }
  }
  constructor(
    private injector: Injector,
    private _screenService: ScreenService,
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject<any>();
  }

  ngOnInit(): void {
    this.loadScreen();
  }

  loadScreen() {
    const param = this.gridConfig.options;
    this._screenService.getGird(param)
    .pipe(takeUntil(this.__unSubscribeAll$))
    .subscribe((response: MvResponse<MvGridResponse<MvScreen>>) => {

    })

  }

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
