import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SchedulesService } from '../../../schedules.service';
import { Subtask } from '../../../schedule';

@Component({
  selector: 'app-schedule-item-subtask-editor',
  templateUrl: './schedule-item-subtask-editor.component.html',
  styleUrls: ['./schedule-item-subtask-editor.component.css']
})
export class ScheduleItemSubtaskEditorComponent implements OnInit {
  @Input() itemId: number;
  @Input() subtask: Subtask;
  @Output() subtaskAdded = new EventEmitter();
  @Output() subtaskUpdated = new EventEmitter();
  @Output() closed = new EventEmitter();
  constructor(private svc: SchedulesService) { }

  ngOnInit() {
  }

  async submit(input: HTMLInputElement) {
    if (!!!input.value.trim()) {
      return this.close();
    }
    if (!!!this.subtask) {
      const id = await this.svc.addSubtask(input.value, this.itemId).toPromise();
      var newSubtask = new Subtask();
      newSubtask.id = id;
      newSubtask.content = input.value;
      newSubtask.dateAdded = new Date();
      this.subtaskAdded.emit(newSubtask);
      input.value = '';
    } else {
      const ok = this.svc.updateSubtask(this.itemId, this.subtask.id, input.value).toPromise();
      if (ok) {
        this.subtaskUpdated.emit(input.value);
      }
    }
  }

  close() {
    this.closed.emit();
  }

  enter(event: KeyboardEvent, input: HTMLInputElement) {
    if (event.code === 'Enter') {
      if (!event.shiftKey) {
        event.preventDefault();
        this.submit(input);
      }
    }
  }

  blur(input: HTMLInputElement) {
    this.submit(input);
  }
}
