import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MenubarModule } from "primeng/menubar";
import { MainLayoutService } from '../../service/main-layout.service';
import { BaseComponent } from '../base.component';

@Component({
  selector: 'main-layout-topbar',
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TopbarComponent extends BaseComponent {

  constructor() 
  {
    super();
  }

  toogleSideBar() {
    this._mainLayoutService.toggleSidebar();
  }
}
