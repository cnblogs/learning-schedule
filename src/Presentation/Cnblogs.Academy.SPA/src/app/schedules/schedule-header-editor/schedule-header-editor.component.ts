import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { SchedulesService } from '../schedules.service';
import { ToastrService } from 'ngx-toastr';
import { Schedule } from '../schedule';

@Component({
  selector: 'app-schedule-header-editor',
  templateUrl: './schedule-header-editor.component.html',
  styleUrls: ['./schedule-header-editor.component.css']
})
export class ScheduleHeaderEditorComponent implements OnInit, AfterViewInit {
  @Input() schedule: Schedule;
  @Input() completed = false;
  @Output() cancel = new EventEmitter();
  @Output() insert = new EventEmitter();
  @Output() update = new EventEmitter();

  title = '';
  isPrivate = false;

  constructor(private svc: SchedulesService,
    private toastr: ToastrService,
    private eleRef: ElementRef) {
    this.schedule = new Schedule();
  }

  ngOnInit() {
    this.title = this.schedule.title;
    this.isPrivate = this.schedule.isPrivate;
  }

  ngAfterViewInit() {
    const el = this.eleRef.nativeElement as HTMLDivElement;
    const input = el.querySelector('.slim') as HTMLInputElement;
    input.focus();
    input.setSelectionRange(input.value.length, input.value.length);
  }

  async save() {
    if (this.schedule.id < 1) {
      await this.addSchedule();
    } else {
      await this.updateSchedule();
    }
  }

  async addSchedule() {
    const schedule = new Schedule();
    schedule.title = this.title;
    schedule.isPrivate = this.isPrivate;
    const result = await this.svc.addSchedule(schedule);
    if (result.success) {
      schedule.id = +result.message;
      schedule.items = [];
      schedule.dateAdded = new Date();
      schedule.dateUpdated = new Date();
      this.insert.emit(schedule);
      this.close();
    } else {
      this.toastr.error(result.message);
    }
    return result.success;
  }

  async updateSchedule() {
    const ok = await this.svc.updateSchedule(this.schedule.id, this.title, this.isPrivate);
    if (ok) {
      this.schedule.title = this.title;
      this.schedule.isPrivate = this.isPrivate;
      this.update.emit(this.schedule);
      this.close();
    } else {
      this.toastr.error('修改失败，请稍后重试');
    }
  }

  close() {
    this.cancel.emit();
  }

  esc(e: any) {
    if (!e.isComposing)
      this.close();
  }
}
