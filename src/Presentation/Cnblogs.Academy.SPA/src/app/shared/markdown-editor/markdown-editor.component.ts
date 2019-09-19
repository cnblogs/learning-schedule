import { Component, OnInit, Input, OnDestroy, Output, EventEmitter } from '@angular/core';
import { MarkdownEditorService } from './markdown-editor.service';

@Component({
  selector: 'app-markdown-editor',
  templateUrl: './markdown-editor.component.html',
  styleUrls: ['./markdown-editor.component.css']
})
export class MarkdownEditorComponent implements OnInit, OnDestroy {
  @Input() id: string;
  @Input() rawContent = '';
  htmlContent: string;
  inRender = false;
  @Output() rawContentChange = new EventEmitter<string>();
  @Output() keyEnter = new EventEmitter();
  @Output() keyEsc = new EventEmitter();
  constructor(private svc: MarkdownEditorService) {

  }

  ngOnInit() {
    this.svc.register(this);
  }

  write() {
    this.inRender = false;
  }

  async preview() {
    const html = await this.svc.toHtml(this.rawContent);
    this.htmlContent = html;
    this.inRender = true;
  }

  onInput(input: string) {
    this.rawContentChange.emit(input);
  }

  onKey(event: KeyboardEvent) {
    if (event.code === 'Enter') {
      if (event.ctrlKey || event.metaKey) {
        this.keyEnter.emit();
      }
    } else if (event.code === 'Escape') {
      this.keyEsc.emit();
    }
  }

  ngOnDestroy() {
    this.svc.remove(this.id);
  }

  close() {
    this.rawContent = '';
    this.htmlContent = '';
    this.inRender = false;
  }
}
