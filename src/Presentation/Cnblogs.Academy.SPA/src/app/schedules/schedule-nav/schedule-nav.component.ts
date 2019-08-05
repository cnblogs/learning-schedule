import { Component, OnInit } from '@angular/core';
import { PermissionService } from '../permission.service';

@Component({
  selector: 'app-schedule-nav',
  templateUrl: './schedule-nav.component.html',
  styleUrls: ['./schedule-nav.component.css']
})
export class ScheduleNavComponent implements OnInit {

  constructor(public permissionSvc: PermissionService) {
  }

  ngOnInit() {
  }

}
