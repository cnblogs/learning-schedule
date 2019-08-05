import { Injectable } from '@angular/core';
import { MarkdownEditorComponent } from './markdown-editor.component';
import { DataService } from '../../infrastructure/data.service';

@Injectable({
  providedIn: 'root'
})
export class MarkdownEditorService {
  private editors: MarkdownEditorComponent[] = [];
  constructor(private dataSvc: DataService) { }

  register(editor: MarkdownEditorComponent) {
    this.editors.push(editor);
  }

  remove(id: string) {
    const i = this.editors.findIndex(x => x.id === id);
    this.editors.splice(i, 1);
  }

  close(id: string) {
    const editor = this.editors.find(x => x.id === id);
    if (editor) {
      editor.close();
    }
  }

  async toHtml(text: string) {
    if (text) {
      return await this.dataSvc.post<string>('api/markdown', text)
        .toPromise()
        .then(x => x.body);
    } else {
      return '这里空空如也';
    }
  }

  getRawContent(id: string): string {
    const editor = this.editors.find(x => x.id === id);
    if (editor) {
      return editor.rawContent;
    }
  }

  getHtmlContent(id: string): string {
    const editor = this.editors.find(x => x.id === id);
    if (editor) {
      return editor.htmlContent;
    }
  }
}
