import { Component, OnInit, Input } from '@angular/core';
import { SchedulesService } from '../../schedules.service';
import { ModalService } from '../../modal/modal.service';
import Swal from 'sweetalert2';
import { Subtask } from '../../schedule';

@Component({
  selector: 'app-schedule-item-subtasks',
  templateUrl: './schedule-item-subtasks.component.html',
  styleUrls: ['./schedule-item-subtasks.component.css'],
  providers: [ModalService]
})
export class ScheduleItemSubtasksComponent implements OnInit {
  @Input() itemId: number;
  @Input() subtasks: Subtask[];
  @Input() owner = false;
  @Input() itemOwner: boolean;
  constructor(private svc: SchedulesService, public modalSvc: ModalService) { }

  ngOnInit() {
  }

  async completeSubtask(task: Subtask, e: Event) {
    e.preventDefault();
    const el = e.target as HTMLInputElement;
    const completed = !!!task.dateEnd;
    const ok = await this.svc.accomplishSubtask(this.itemId, task.id, completed).toPromise();
    if (ok) {
      task.dateEnd = completed ? new Date() : null;
      el.checked = completed;
    } else {
      el.checked = !completed;
    }
  }

  addSubtask(subtask: Subtask) {
    this.subtasks.push(subtask);
  }

  updateSubtask(newContent: string, subtask: Subtask) {
    subtask.content = newContent;
  }

  async delSubtask(subtask: Subtask) {
    const will = await Swal.fire({
      title: `将永久删除「${subtask.content}」`, text: '删除后不可撤销',
      cancelButtonText: '取消',
      confirmButtonText: '删除',
      type: 'warning',
      showCancelButton: true,
      focusCancel: true
    });
    if (!will.value) return;
    const ok = this.svc.deleteSubtask(subtask.id).toPromise();
    if (ok) {
      this.subtasks = this.subtasks.filter(x => x !== subtask);
    }
  }

  edit(subtaskId: string) {
    if (this.itemOwner) {
      this.modalSvc.open(subtaskId);
    }
  }
}
