import { Component, OnDestroy, OnInit } from '@angular/core';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { mediaColumn } from '../../model/media-column';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-media-list',
  templateUrl: './media-list.component.html',
  styleUrl: './media-list.component.scss',
})
export class MediaListComponent implements OnInit, OnDestroy {
  private __unSubscribeAll : Subject<any>;
  gridConfig: GridConfig = {
    column: mediaColumn,
    dataSource: {
      data: [],
      totalRows: 0,
    },
    options: {},
  };

  constructor()
  {
    this.__unSubscribeAll = new Subject();
  }

  ngOnInit(): void {
    
  }

  ngOnDestroy(): void {
    this.__unSubscribeAll.next(null);
    this.__unSubscribeAll.complete();
  }
}
