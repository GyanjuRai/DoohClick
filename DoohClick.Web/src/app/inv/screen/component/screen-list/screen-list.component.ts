import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { AppComponent } from '../../../../app.component';
import { Subject } from 'rxjs';
import { ScreenService } from '../../service/screen.service';

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
  constructor(
    private injector: Injector,
    private _screenService: ScreenService,
  ) {
    super(injector);
    this.__unSubscribeAll$ = new Subject<any>();
  }

  ngOnInit(): void {}

  loadScreen() {}

  ngOnDestroy(): void {
    this.__unSubscribeAll$.next(null);
    this.__unSubscribeAll$.complete();
  }
}
