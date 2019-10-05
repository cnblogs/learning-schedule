import { Component, OnInit, Output, EventEmitter, Input, ViewChild, ElementRef } from '@angular/core';
import { Schedule, ScheduleDetail } from '../schedule';
import { ToastrService } from 'ngx-toastr';
import { ModalService } from '../modal/modal.service';
import { PanMenuService } from '../pan-menu/pan-menu.service';
import Swal from 'sweetalert2';
import { AuthService } from '../../services/auth.service';
import * as moment from 'moment';
import { map } from 'rxjs/operators';
import { SchedulesService } from '../schedules.service';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css'],
  providers: [
    ModalService
  ]
})
export class ScheduleComponent implements OnInit {
  @Input() schedule: ScheduleDetail;

  @Output() update = new EventEmitter();
  @Output() delete = new EventEmitter();

  constructor(
    private svc: SchedulesService,
    private toastr: ToastrService,
    public modalSvc: ModalService,
    public panSvc: PanMenuService,
    private authSvc: AuthService) { }

  ngOnInit() {
  }

  @ViewChild('block', { static: false }) block: ElementRef;

  get completed() {
    return this.schedule.dateEnd;
  }

  async onClick(event: Event) {
    if (!!!this.schedule.dateEnd) {
      await this.complete(event);
    } else {
      await this.cancelComplete(event);
    }
  }

  async complete(event: Event) {
    let ok = false;
    const will = await Swal.fire({
      text: '确定要标记为完成吗？',
      confirmButtonText: '完成',
      showCancelButton: true,
      cancelButtonText: '取消',
      type: 'question'
    });
    if (will.value) {
      ok = true;
      if (!this.schedule.items.every(x => x.completed)) {
        const still = await Swal.fire({
          text: '还有任务尚未完成，确定要继续吗？',
          cancelButtonText: '取消',
          confirmButtonText: '继续',
          showCancelButton: true,
          type: 'question'
        });
        if (!still.value) {
          ok = false;
        }
      }
    }
    if (ok) {
      const result = await this.svc.complete(this.schedule.id);
      if (result.success) {
        this.schedule.dateEnd = new Date();
        return;
      } else {
        this.toastr.error(result.message);
      }
    }
    if (event) {
      const el = event.target as HTMLInputElement;
      el.checked = false;
    }
  }

  async cancelComplete(e: Event) {
    const will = await Swal.fire({
      text: '确定要取消完成吗？',
      showCancelButton: true,
      cancelButtonText: '取消',
      confirmButtonText: '确定',
      type: 'question'
    });
    if (will.value) {
      const ok = await this.svc.cancelComplete(this.schedule.id);
      if (ok) {
        this.schedule.dateEnd = null;
        return;
      }
    }
    if (e) {
      const el = e.target as HTMLInputElement;
      el.checked = true;
    }
  }

  openEdit(modalId: string) {
    if (this.schedule.parentId > 0) {
      return Swal.fire('很抱歉', '借鉴来的学习计划不允许修改');
    }
    this.modalSvc.open(modalId);
  }

  async updateSchedule(schedule: Schedule) {
    this.schedule.title = schedule.title;
    this.schedule.isPrivate = schedule.isPrivate;
  }

  async delSchedule(id: number) {
    const will = await Swal.fire({
      text: '确定要删除这个目标吗？',
      cancelButtonText: '取消',
      confirmButtonText: '删除',
      type: 'warning',
      showCancelButton: true,
      focusCancel: true,
      customClass: {
        confirmButton: 'swal2-danger'
      }
    });
    if (will.value) {
      const ok = await this.svc.deleteSchedule(id);
      if (ok) {
        this.toastr.success('已删除');
        this.delete.emit(id);
      } else {
        this.toastr.error('删除失败，请稍后重试');
      }
    }
  }

  scrollCenter() {
    this.block.nativeElement.scrollIntoView({ block: 'center', behavior: 'smooth' });
  }

  async togglePrivate(schedule: Schedule) {
    const tip = '24小时内只能切换一次';
    const time = await this.svc.getLastUpdatePrivate(schedule.id) || moment().add(-2, 'days');
    moment.locale('zh-cn');
    const timeHum = moment(time).fromNow();
    const disabled = moment(time).isAfter(moment().add(-1, 'days'));
    if (disabled) {
      return Swal.fire({ text: `${tip}，上次操作 ${timeHum}`, type: 'info' });
    } else {
      const will = await Swal.fire({
        text: `${tip}，确定要切换为${schedule.isPrivate ? '公开' : '私有'}吗`,
        cancelButtonText: '取消',
        confirmButtonText: '切换',
        showCancelButton: true,
        type: 'warning'
      });
      if (will.value) {
        this.svc.updatePrivate(schedule.id, !schedule.isPrivate).subscribe(x => {
          if (x.ok) {
            schedule.isPrivate = !schedule.isPrivate;
          } else {
            Swal.fire({ text: `很抱歉切换为${schedule.isPrivate ? '公开' : '私有'} 失败，请稍后重试`, type: 'error' });
          }
        });
      }
    }
  }

  isScheduleOwner() {
    return this.authSvc.getPrivacy().pipe(
      map(x => x.value.alias === this.schedule.user.alias)
    );
  }

  async panStart(ele: any, event: any) {
    if (await this.isScheduleOwner().toPromise()) {
      this.panSvc.panstart(ele, event)
    }
  }

  async panMove(event: any, ele: any) {
    if (await this.isScheduleOwner().toPromise()) {
      this.panSvc.panmove(event, ele);
    }
  }

  async panEnd(ele: any) {
    if (await this.isScheduleOwner().toPromise()) {
      this.panSvc.panend(ele);
    }
  }
}
