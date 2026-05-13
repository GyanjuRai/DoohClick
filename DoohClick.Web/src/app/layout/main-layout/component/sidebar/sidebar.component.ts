import {
  ChangeDetectionStrategy,
  Component,
} from '@angular/core';
import { MainLayoutService } from '../../service/main-layout.service';
import { Observable } from 'rxjs';
import { BaseComponent } from '../base.component';
import { ROUTE_PATHS } from '../../../../shared';
import { Router } from '@angular/router';

@Component({
  selector: 'main-layout-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SidebarComponent extends BaseComponent {
  sideBarVisible: Observable<boolean> = this._mainLayoutService.sideBar$;
  loginUrl = `${ROUTE_PATHS.AUTH}/${ROUTE_PATHS.LOGIN}`;
  constructor(
    private router: Router
  ) 
  {
    super();
  }

  logout() {
    this._auth.clearAuth();
    this.router.navigate([this.loginUrl]);
  }
}
