import { Component, OnInit, OnDestroy, Inject, PLATFORM_ID } from '@angular/core';
import { SchedulesService} from '../schedules.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { getRouterParam } from '../../infrastructure/paramFetcher';
import { ScheduleDetail } from '../schedule';

@Component({
  selector: 'app-schedule-detail',
  templateUrl: './schedule-detail.component.html',
  styleUrls: ['./schedule-detail.component.css']
})
export class ScheduleDetailComponent implements OnInit, OnDestroy {
  alias: string;
  scheduleId: number;
  schedule: ScheduleDetail;
  isOwner = false;
  deleted = false;

  constructor(private svc: SchedulesService,
    private route: ActivatedRoute,
    private authSvc: AuthService,
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId) {
    this.schedule = new ScheduleDetail();
    this.schedule.id = -1;
    this.alias = getRouterParam(route.snapshot.root);
    this.scheduleId = +route.snapshot.params['scheduleId'];
  }

  async ngOnInit() {
    this.schedule = await this.svc.getSchedule(this.alias, this.scheduleId);
    this.authSvc.getPrivacy().subscribe(x => {
      if (x.success) {
        this.isOwner = x.value.alias === this.alias;
      }
    });
  }

  ngOnDestroy(): void {
    try {
      if (isPlatformBrowser(this.platformId)) {
        const prefix = location.host.startsWith('dev') ? 'dev-' : '';
        this.http.put(`${location.protocol}//${prefix}count.cnblogs.com/academy/schedule/${this.scheduleId}`, null,
          { withCredentials: true }).subscribe(
            () => { }, (err) => {
              console.log(err);
            });
      }
    } catch (ex) {
      console.log(ex);
    }
  }
}
