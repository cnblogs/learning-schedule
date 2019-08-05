import { Component, OnInit, Input } from '@angular/core';
import { SchedulesService } from '../../schedules.service';
import { ModalService } from '../../modal/modal.service';
import Swal from 'sweetalert2';
import { Reference } from '../../schedule';

@Component({
  selector: 'app-schedule-item-references',
  templateUrl: './schedule-item-references.component.html',
  styleUrls: ['./schedule-item-references.component.css'],
  providers: [ModalService]
})
export class ScheduleItemReferencesComponent implements OnInit {
  @Input() itemId: number;
  @Input() references: Reference[];
  @Input() owner: boolean = false;
  @Input() itemOwner: boolean = false;

  constructor(private svc: SchedulesService, public modalSvc: ModalService) { }

  ngOnInit() {
  }

  add(ref: Reference) {
    this.references.push(ref);
  }

  async delete(ref: Reference) {
    const will = await Swal.fire({
      title: `将永久删除「${ref.url}」`, text: '删除后不可恢复',
      cancelButtonText: '取消',
      confirmButtonText: '删除',
      type: 'warning',
      focusCancel: true
    });
    if (!will.value) return;

    const ok = this.svc.deleteReference(ref.id).toPromise();
    if (ok) {
      this.references = this.references.filter(x => x !== ref);
    }
  }

  subHost(link: string) {
    try {
      const url = new URL(link);
      return url.host;
    } catch (error) {
      return link;
    }
  }

  update(url: string, ref: Reference) {
    ref.url = url;
  }

  open(modalId: string) {
    if (this.itemOwner) {
      this.modalSvc.open(modalId);
    }
  }
}
