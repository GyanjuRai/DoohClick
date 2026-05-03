import { Component } from '@angular/core';
import { GridConfig } from '../../../../shared/model/grid-config.model';
import { mediaColumn } from '../../model/media-column';

@Component({
  selector: 'app-media-list',
  templateUrl: './media-list.component.html',
  styleUrl: './media-list.component.scss',
})
export class MediaListComponent {
  gridConfig: GridConfig = {
    column: mediaColumn,
    dataSource: {
      data: [],
      totalRows: 0,
    },
    options: {},
  };
}
