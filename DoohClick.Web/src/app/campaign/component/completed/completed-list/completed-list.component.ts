import { Component, OnInit } from '@angular/core';
import { CampaignBaseClass } from '../../campaign';

@Component({
  selector: 'app-completed-list',
  templateUrl: './completed-list.component.html',
  styleUrl: './completed-list.component.scss'
})
export class CompletedListComponent extends CampaignBaseClass implements OnInit {

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.loadCampaing('COMPLETED');
  }
}
