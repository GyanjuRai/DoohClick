import { Component, OnInit } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';

@Component({
  selector: 'app-scheduled-list',
  templateUrl: './scheduled-list.component.html',
  styleUrl: './scheduled-list.component.scss'
})
export class ScheduledListComponent extends CampaignBaseClass implements OnInit {

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.loadCampaing('SCHEDULED');
  }
}