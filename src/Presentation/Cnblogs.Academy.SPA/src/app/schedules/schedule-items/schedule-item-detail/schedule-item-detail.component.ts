import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ScheduleItemDetail, ScheduleItem } from '../../schedule';
import { PermissionService } from '../../permission.service';
import { SwitcherService } from '../../switcher.service';
import { ModalService } from '../../modal/modal.service';
import { getRouterParam } from '../../../infrastructure/paramFetcher';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import { SchedulesService } from '../../schedules.service';

@Component({
  selector: 'app-schedule-item-detail',
  templateUrl: './schedule-item-detail.component.html',
  styleUrls: ['./schedule-item-detail.component.css'],
  providers: [ModalService]
})
export class ScheduleItemDetailComponent implements OnInit, OnDestroy {
  scheduleId: number;
  itemId: number;
  detail: ScheduleItemDetail;

  @Output() delete = new EventEmitter();

  constructor(private route: ActivatedRoute,
    private router: Router,
    private svc: SchedulesService,
    public permissionSvc: PermissionService,
    private switcherSvc: SwitcherService,
    public modalSvc: ModalService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.scheduleId = +getRouterParam(this.route.snapshot.root, 'scheduleId');
    this.route.params.subscribe(async x => {
      this.itemId = +x['itemId'];
      this.detail = await this.svc.getScheduleItemDetail(this.itemId).toPromise();
    });
    this.switcherSvc.setDetailPannel(true);
  }

  close() {
    this.router.navigate(['../../../'], { relativeTo: this.route });
  }

  get IsItemOwner() {
    return this.permissionSvc.isCurrentUser(this.detail.user.alias);
  }

  ngOnDestroy() {
    this.switcherSvc.setDetailPannel(false);
  }

  updateTitle(item: ScheduleItem) {
    this.switcherSvc.changeItemTitleSource(item);
  }

  async deleteItem() {
    const will = await Swal.fire({
      text: '确定要删除这个任务吗？',
      showCancelButton: true,
      cancelButtonText: '取消',
      confirmButtonText: '删除',
      type: 'warning',
      focusCancel: true,
      customClass: {
        confirmButton: 'swal2-danger'
      }
    });
    if (will.value) {
      const ok = await this.svc.delItem(this.itemId);
      if (ok) {
        this.delete.emit(this.itemId);
        this.switcherSvc.changeItemDeletionSource(this.itemId);
        this.close();
      } else {
        this.toastr.error('删除失败，请稍后重试');
      }
    }
  }

  async open(modalId: string) {
    const owner = await this.IsItemOwner.toPromise();
    if (owner) {
      this.modalSvc.open(modalId);
    }
  }
}
