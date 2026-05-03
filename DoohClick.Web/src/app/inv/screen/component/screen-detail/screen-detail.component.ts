import { Component } from '@angular/core';
import { MvScreen } from '../../model/screen.model';

@Component({
  selector: 'screen-detail',
  templateUrl: './screen-detail.component.html',
  styleUrl: './screen-detail.component.scss',
})
export class ScreenDetailComponent {
  protected isDialogOpen: boolean = false;
  protected screen: MvScreen = {} as MvScreen;

  public openDialog(screen: MvScreen) {
    this.screen = screen;
    this.isDialogOpen = true;
  }

  protected close() {
    this.isDialogOpen = false;
  }
}
