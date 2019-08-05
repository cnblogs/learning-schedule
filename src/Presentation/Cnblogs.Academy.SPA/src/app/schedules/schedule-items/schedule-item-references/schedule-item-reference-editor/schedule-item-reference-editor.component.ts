import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SchedulesService } from '../../../schedules.service';
import * as clipboard from "clipboard-polyfill"
import { Reference } from '../../../schedule';

@Component({
  selector: 'app-schedule-item-reference-editor',
  templateUrl: './schedule-item-reference-editor.component.html',
  styleUrls: ['./schedule-item-reference-editor.component.css']
})
export class ScheduleItemReferenceEditorComponent implements OnInit {
  @Input() itemId: number;
  @Input() reference: Reference;
  @Output() referenceAdded = new EventEmitter();
  @Output() referenceUpdated = new EventEmitter();
  @Output() closed = new EventEmitter();
  alert: string;
  constructor(private svc: SchedulesService) { }

  ngOnInit() {
  }

  focus(input: HTMLInputElement) {
    clipboard.readText().then(x => {
      if (x) {
        try {
          const url = new URL(x);
          input.value = url.toString();
        } catch (error) {
          console.log('Not validated url');
        }
      }
    }).catch(x => {
      console.log(x);
    })
  }

  async submit(input: HTMLInputElement) {
    this.alert = '';
    if (!!!input.value.trim()) {
      return this.close();
    }
    try {
      new URL(input.value);
    } catch (error) {
      this.alert = "请输入正确的链接地址";
      return;
    }
    if (!!!this.reference) {
      const id = await this.svc.addRef(input.value, this.itemId).toPromise();
      var newRef = new Reference();
      newRef.id = id;
      newRef.url = input.value;
      newRef.dateAdded = new Date();
      this.referenceAdded.emit(newRef);
      input.value = '';
    } else {
      const ok = await this.svc.updateReference(this.itemId, this.reference.id, input.value).toPromise();
      if (ok) {
        this.referenceUpdated.emit(input.value);
      }
    }
  }

  enter(event: KeyboardEvent, input: HTMLInputElement) {
    if (event.code === 'Enter') {
      if (!event.shiftKey) {
        event.preventDefault();
        this.submit(input);
      }
    }
  }

  close() {
    this.closed.emit();
  }

  blur(input: HTMLInputElement) {
    this.submit(input);
  }
}
