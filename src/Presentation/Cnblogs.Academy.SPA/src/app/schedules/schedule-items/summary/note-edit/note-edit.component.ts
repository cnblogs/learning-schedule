import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SchedulesService } from 'src/app/schedules/schedules.service';
import { SummaryNote } from 'src/app/schedules/schedule';
import { MarkdownEditorService } from 'src/app/shared/markdown-editor/markdown-editor.service';

@Component({
  selector: 'app-note-edit',
  templateUrl: './note-edit.component.html',
  styleUrls: ['./note-edit.component.scss']
})
export class NoteEditComponent implements OnInit {
  @Input() itemId: number;
  @Input() note: SummaryNote;
  isUpdated = false;
  @Output() closed = new EventEmitter();
  @Output() saved = new EventEmitter();
  constructor(private svc: SchedulesService, private markdown: MarkdownEditorService) { }

  ngOnInit() {
    if (!!!this.note) {
      this.note = new SummaryNote();
    } else if (this.note.id > 0) {
      this.isUpdated = true;
    }
  }

  onChange(input: string) {
    this.note.note = input;
  }

  async save() {
    if (!this.isUpdated) {
      this.svc.addSummaryNote(this.itemId, this.note).subscribe(x => {
        this.render();
        this.note.id = x.body;
        this.saved.emit(this.note);
      })
    } else {
      this.svc.updateSummaryNote(this.itemId, this.note).subscribe(x => {
        this.render();
        this.closed.emit();
      })
    }
  }

  async render() {
    this.note.html = await this.markdown.toHtml(this.note.note);
  }

  cancel() {
    this.closed.emit();
  }
}
