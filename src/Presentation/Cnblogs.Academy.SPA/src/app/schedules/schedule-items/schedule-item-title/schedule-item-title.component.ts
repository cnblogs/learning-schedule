import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ScheduleItem } from '../../schedule';
import { PanMenuService } from '../../pan-menu/pan-menu.service';
import { ToastrService } from 'ngx-toastr';
import { PermissionService } from '../../permission.service';
import { SchedulesService } from '../../schedules.service';
import { ModalService } from '../../modal/modal.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-schedule-item-title',
  templateUrl: './schedule-item-title.component.html',
  styleUrls: ['./schedule-item-title.component.css'],
  providers: [
    ModalService
  ]
})
export class ScheduleItemTitleComponent implements OnInit {
  @Input() scheduleId: number;
  @Input() item: ScheduleItem;
  @Input() disabled: boolean;
  @Output() update = new EventEmitter();
  @Output() open = new EventEmitter();
  @Output() delete = new EventEmitter();
  summary_open = false;
  constructor(
    public panSvc: PanMenuService,
    private svc: SchedulesService,
    private toastr: ToastrService,
    public permissionsvc: PermissionService,
    public modalSvc: ModalService) { }

  ngOnInit() {
  }

  async todoItem(item: ScheduleItem, event: Event) {
    if (event) {
      event.preventDefault();
    }
    if (this.disabled) { return false; }
    try {
      if (!item.completed) {
        const result = await this.svc.todoItem(this.scheduleId, item);
        if (result.success) {
          item.completed = true;
          item.dateEnd = new Date();
          this.toastr.success('任务完成');
          this.update.emit(item);
        }
      } else {
        const result = await this.svc.delRecord(this.scheduleId, item.id);
        if (result.success) {
          item.completed = false;
          this.toastr.success('已取消');
          this.summary_open = false;
          this.update.emit(item);
        } else {
          this.toastr.error(result.message);
        }
      }
    } catch {
    } finally {
      if (event) {
        (<HTMLInputElement>event.target).checked = item.completed;
      }
    }
  }

  async delItem() {
    if (this.item.parentId > 0) {
      return Swal.fire('很抱歉', '借鉴来的学习任务不允许删除');
    }
    const will = await Swal.fire({
      text: '确定要删除这个任务吗？',
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
      const ok = await this.svc.delItem(this.item.id);
      if (ok) {
        this.delete.emit(this.item.id);
      } else {
        this.toastr.error('删除失败，请稍后重试');
      }
    }
  }

  openEdit(modelId: string) {
    if (this.item.parentId > 0) {
      return Swal.fire('很抱歉', '借鉴来的学习任务不允许编辑');
    }
    this.modalSvc.open(modelId);
  }

  async panStart(ele: any, event: any) {
    if (!this.disabled) {
      this.panSvc.panstart(ele, event)
    }
  }

  async panMove(event: any, ele: any) {
    if (!this.disabled) {
      this.panSvc.panmove(event, ele);
    }
  }

  async panEnd(ele: any) {
    if (!this.disabled) {
      this.panSvc.panend(ele);
    }
  }

  openSummaryPannel() {
    this.summary_open = !this.summary_open;
  }

}
