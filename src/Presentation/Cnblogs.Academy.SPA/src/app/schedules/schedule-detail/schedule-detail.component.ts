import { Component, OnInit, OnDestroy, Inject, PLATFORM_ID } from '@angular/core';
import { SchedulesService } from '../schedules.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { getRouterParam } from '../../infrastructure/paramFetcher';
import { ScheduleDetail, Following, AcademyUser } from '../schedule';
import Swal from 'sweetalert2';
import { GlobalStatusService } from 'src/app/shared/global-status.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-schedule-detail',
  templateUrl: './schedule-detail.component.html',
  styleUrls: ['./schedule-detail.component.scss']
})
export class ScheduleDetailComponent implements OnInit, OnDestroy {
  alias: string;
  scheduleId: number;
  schedule: ScheduleDetail;
  isOwner = false;
  deleted_open = false;
  followings: Following[];
  following_open = false;
  loading = false;

  constructor(private svc: SchedulesService,
    private route: ActivatedRoute,
    private router: Router,
    private authSvc: AuthService,
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId,
    public globalSvc: GlobalStatusService) {
    this.schedule = new ScheduleDetail();
    this.schedule.id = -1;
    this.globalSvc.clickRoot$.subscribe(x => {
      this.following_open = false;
    });
  }

  async ngOnInit() {
    this.route.params.subscribe(async params => {
      this.loading = true;
      this.alias = getRouterParam(this.route.snapshot.root);
      this.scheduleId = +params['scheduleId'];
      this.schedule = await this.svc.getSchedule(this.alias, this.scheduleId).pipe(
        finalize(() => { this.loading = false; })
      ).toPromise();
      this.loading = false;
      this.deleted_open = false;
      this.authSvc.getPrivacy().subscribe(x => {
        if (x.success) {
          this.isOwner = x.value.alias === this.alias;
        }
      });
      this.svc.getFollowings(this.schedule.id).subscribe(x => {
        this.followings = x;
        this.setFollowingIcons(this.followings);
      });
    })
  }

  followingIcons: string[] = [];
  setFollowingIcons(followings: Following[]) {
    if (!!!followings) return null;
    for (let index = 0; index < followings.length; index++) {
      const element = followings[index];
      this.followingIcons.push(element.user.icon);
      if (index > 8) {
        return;
      }
    }
  }

  ngOnDestroy(): void {
    try {
      if (isPlatformBrowser(this.platformId)) {
        const prefix = location.host.startsWith('dev') ? 'dev-' : '';
        this.http.put(`${location.protocol}//${prefix}count.cnblogs.com/academy/schedule/${this.scheduleId}`, null,
          { withCredentials: true }).subscribe(
            () => { }, (err) => {
              console.error(err);
            });
      }
    } catch (ex) {
      console.error(ex);
    }
  }

  async subscribe() {
    Swal.queue([{
      type: 'info',
      title: '借鉴学习计划',
      text: '借鉴之后会复制一份到自己的计划中，并会同步更新学习任务',
      showConfirmButton: true,
      showCancelButton: true,
      confirmButtonText: '借鉴',
      cancelButtonText: '取消',
      focusConfirm: true,
      showLoaderOnConfirm: true,
      allowOutsideClick: false,
      allowEscapeKey: false,
      preConfirm: () => {
        return this.svc.subscribe(this.schedule.id).toPromise().then(async x => {
          if (x.ok) {
            if (x.body < 1) {
              Swal.insertQueueStep({ text: '借鉴失败，请稍后重试' });
              return;
            }
            this.schedule.followingCount += 1;
            this.followings = this.followings || [];
            const following = new Following();
            following.scheduleId = x.body;
            const user = this.authSvc.authResult.value;
            following.user = new AcademyUser(user.alias, user.icon, user.name);
            this.followings.push(following);

            const will = await Swal.fire({
              type: 'success',
              text: '已成功借鉴该计划',
              showCloseButton: true,
              showCancelButton: true,
              focusConfirm: false,
              confirmButtonText: '现在去查看',
              cancelButtonText: '稍后再看'
            });
            if (will.value) {
              this.router.navigate(['/schedules/u/', this.authSvc.authResult.value.alias, x.body]);
            }
          }
        }).catch(x => {
          Swal.close();
        });
      }
    }]);
  }
}
