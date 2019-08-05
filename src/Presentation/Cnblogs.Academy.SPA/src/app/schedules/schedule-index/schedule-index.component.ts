import { Component, OnInit, OnDestroy } from '@angular/core';
import { ScheduleDetail, AcademyUser } from '../schedule';
import { PaginationInstance } from 'ngx-pagination';
import { ActivatedRoute } from '@angular/router';
import { ModalService } from '../modal/modal.service';
import { PanMenuService } from '../pan-menu/pan-menu.service';
import { SwitcherService } from '../switcher.service';
import { Subscription } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { getRouterParam } from '../../infrastructure/paramFetcher';
import { PermissionService } from '../permission.service';
import { SchedulesService } from '../schedules.service';

@Component({
  selector: 'app-schedule-index',
  templateUrl: './schedule-index.component.html',
  styleUrls: ['./schedule-index.component.css'],
  providers: [
    ModalService
  ]
})
export class ScheduleIndexComponent implements OnInit, OnDestroy {
  list: ScheduleDetail[];
  config: PaginationInstance = {
    id: 'schedule-index-page',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 10
  };
  teachOnly = false;
  completedOnly = false;
  alias: string;
  private subscription: Subscription;

  constructor(
    private svc: SchedulesService,
    private route: ActivatedRoute,
    public modalSvc: ModalService,
    public panSvc: PanMenuService,
    switcherSvc: SwitcherService,
    private authSvc: AuthService,
    public permissionSvc: PermissionService) {
    this.subscription = switcherSvc.newoneModal$.subscribe(x => {
      this.modalSvc.toggle('newone');
    });
    this.teachOnly = this.permissionSvc.inTeach;
    this.completedOnly = route.snapshot.url.some(x => x.path === 'done');
    this.alias = getRouterParam(route.snapshot.root);
  }

  async ngOnInit() {
    if (!!!this.alias) {
      const result = await this.authSvc.getPrivacy().toPromise();
      this.alias = result.value.alias;
    }
    this.route.queryParams.subscribe(async x => {
      const p = +x['page'] || 1;
      this.config.currentPage = p;
      const result = await this.svc.listWithItems(this.alias, this.completedOnly, this.teachOnly, this.config.currentPage, this.config.itemsPerPage);
      this.config.totalItems = +result.totalCount;
      this.list = result.items;

      const open = this.route.snapshot.queryParams['open'];
      if (open) {
        const id = setTimeout(() => {
          this.modalSvc.open('newone');
          clearTimeout(id);
        }, 10);
      }
    });
  }

  async addSchedule(schedule: ScheduleDetail) {
    schedule.user = AcademyUser.CreateUser(this.authSvc.authResult.value);
    this.list.unshift(schedule);
    this.config.totalItems += 1;
    const id = setTimeout(() => {
      this.modalSvc.open('n' + schedule.id);
      clearTimeout(id);
    }, 10);
  }

  async delSchedule(id: number) {
    this.list = this.list.filter(x => x.id !== id);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
