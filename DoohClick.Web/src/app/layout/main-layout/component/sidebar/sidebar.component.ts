import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';
import { MainLayoutService } from '../../service/main-layout.service';
import { Observable } from 'rxjs';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'main-layout-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent extends BaseComponent {
  sideBarVisible: Observable<boolean> = this._mainLayoutService.sideBar$;

  constructor() 
  {
    super();
  }

}
