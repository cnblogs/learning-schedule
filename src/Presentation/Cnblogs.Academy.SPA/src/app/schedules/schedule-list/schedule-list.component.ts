import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { ScheduleDetail } from '../schedule';
import { PaginationInstance } from 'ngx-pagination';
import { SchedulesService } from '../schedules.service';

@Component({
  selector: 'app-schedule-list',
  templateUrl: './schedule-list.component.html',
  styleUrls: ['./schedule-list.component.css']
})
export class ScheduleListComponent implements OnInit {
  @Input() schedules: ScheduleDetail[];
  @Input() config: PaginationInstance = {
    id: 'schedule-list-paging',
    currentPage: 1,
    itemsPerPage: 30,
    totalItems: 30
  };
  @Output() pageChanged = new EventEmitter();
  constructor(private svc: SchedulesService) { }

  ngOnInit() {
  }

  get groups() {
    return this.svc.groupByDate(this.schedules);;
  }

}
