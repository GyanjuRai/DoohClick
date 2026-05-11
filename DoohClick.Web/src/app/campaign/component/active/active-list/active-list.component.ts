import { Component } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';

@Component({
  selector: 'app-active-list',
  templateUrl: './active-list.component.html',
  styleUrl: './active-list.component.scss'
})
export class ActiveListComponent extends CampaignBaseClass {

  constructor() {
    super();
  }
}
