import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit, ElementRef } from '@angular/core';
import { ScheduleItem, AcademyUser } from '../../schedule';
import { MarkdownEditorService } from '../../../shared/markdown-editor/markdown-editor.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../services/auth.service';
import { SchedulesService } from '../../schedules.service';

@Component({
  selector: 'app-schedule-item-editor',
  templateUrl: './schedule-item-editor.component.html',
  styleUrls: ['./schedule-item-editor.component.css']
})
export class ScheduleItemEditorComponent implements OnInit, AfterViewInit {
  @Input() scheduleId: number;
  @Input() item: ScheduleItem;
  @Output() cancel = new EventEmitter();
  @Output() insert = new EventEmitter();
  @Output() update = new EventEmitter();

  constructor(
    private svc: SchedulesService,
    private markdownSvc: MarkdownEditorService,
    private toastr: ToastrService,
    private eleRef: ElementRef,
    private authSvc: AuthService) {
    this.item = new ScheduleItem();
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    const el = this.eleRef.nativeElement as HTMLDivElement;
    const textarea = el.querySelector('textarea');
    textarea.focus();
    textarea.setSelectionRange(textarea.value.length, textarea.value.length);
  }

  esc(e: any) {
    if (!e.isComposing)
      this.close();
  }

  close() {
    this.cancel.emit();
  }

  async save() {
    if (this.item.id < 1) {
      await this.addItem(this.item.title)
    } else {
      await this.updateItem(this.item.title)
    }
  }

  async addItem(input: string) {
    const id = await this.svc.addMarkdownItem(this.scheduleId, input);
    if (id && id > 0) {
      this.item.id = id;
      this.item.dateAdded = new Date();
      this.item.html = await this.markdownSvc.toHtml(input);
      if (this.authSvc.authResult.success) {
        this.item.user = AcademyUser.CreateUser(this.authSvc.authResult.value);
        this.insert.emit(this.item);
      }
      return true;
    } else {
      this.toastr.error('添加任务失败，请稍后重试');
    }
    return false;
  }

  async updateItem(input: string) {
    const ok = await this.svc.updateItem(this.item.id, input);
    if (ok) {
      this.item.title = input;
      this.item.html = await this.markdownSvc.toHtml(input);
      this.update.emit(this.item);
      return true;
    } else {
      this.toastr.error('修改失败，请稍后重试');
    }
    return false;
  }

  onEnter(e: KeyboardEvent, btn: HTMLButtonElement) {
    if (!e.shiftKey) {
      e.preventDefault();
      btn.click();
    }
  }

  blur(btn: HTMLButtonElement) {
    btn.click();
  }
}
