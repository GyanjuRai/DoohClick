import { Component } from '@angular/core';
import { BaseComponent } from './base.component';

@Component({
  selector: 'main-layout',
  template: `
    <main-layout-sidebar></main-layout-sidebar>
    <div
      class="layout-main-container"
      [class.sidebar-open]="isSidebarVisible$ | async"
    >
      <main-layout-topbar></main-layout-topbar>
      <div class="layout-container">
        <router-outlet></router-outlet>
      </div>
    </div>
  `,
  styles: [`
      .layout-main-container {
        transition: margin-left 0.2s cubic-bezier(0, 0, 0.2, 1);
        min-height: 95vh;
        display: flex;
        flex-direction: column;
      }

      .layout-main-container.sidebar-open {
        margin-left: var(--sidebar-width) !important;
      }

      .layout-container {
        flex: 1;
        background-color: var(--surface-50) !important;
      }
    `]
})
export class MainLayoutComponent extends BaseComponent {
  isSidebarVisible$ = this._mainLayoutService.sideBar$;
}
