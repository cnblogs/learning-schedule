import { Component, OnInit, Input } from '@angular/core';
import { SchedulesService } from '../../schedules.service';
import { Summary } from '../../schedule';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss']
})
export class SummaryComponent implements OnInit {
  @Input() itemId: number;
  @Input() readonly = true;
  summary: Summary;
  constructor(private svc: SchedulesService) { }

  ngOnInit() {
    this.svc.getSummary(this.itemId).subscribe(x => {
      this.summary = x;
    })
  }

}
