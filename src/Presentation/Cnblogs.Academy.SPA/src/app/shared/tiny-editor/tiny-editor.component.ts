import {
  Component,
  EventEmitter,
  Input,
  Output,
  forwardRef
} from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-tiny-editor',
  template: `<editor
  hostUrl="assets/tinymce/tinymce.min.js"
  [init]="{
    plugins: ['print', 'preview', 'searchreplace', 'codesample', 'link', 'autolink', 'table', 'fullscreen',
  'image', 'media', 'paste', 'toc', 'autosave', 'contextmenu'],
    language: 'zh_CN',
    height: height,
    images_upload_handler: upload_image}"
    [(ngModel)]="content"></editor>`,
  providers: [
    { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => TinyEditorComponent), multi: true }
  ]
})

export class TinyEditorComponent implements ControlValueAccessor {
  @Input() height = 250;

  private _content: string;
  public get content(): string {
    return this._content;
  }
  public set content(v: string) {
    if (!!!v) {
      v = '';
    }
    this._content = v;
    this.propagateChange(v);
  }

  propagateChange: any = () => { };

  writeValue(obj: any): void {
    this.content = obj;
  }
  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }
  registerOnTouched(fn: any): void { }

  upload_image(blobInfo: any, success: any, failure: any) {
    let xhr: XMLHttpRequest, formData;
    xhr = new XMLHttpRequest();
    xhr.withCredentials = true;
    xhr.open('POST', '//upload.cnblogs.com/ImageUploader/ProcessUpload?host=academy.cnblogs.com');
    xhr.onload = function () {
      let json;

      if (xhr.status !== 200) {
        failure('HTTP Error: ' + xhr.status);
        return;
      }
      json = JSON.parse(xhr.responseText);

      if (json && json.success && json.message) {
        success(json.message);
      } else {
        failure('Invalid JSON: ' + xhr.responseText);
      }
    };
    formData = new FormData();
    formData.append('file', blobInfo.blob(), blobInfo.filename());
    xhr.send(formData);
  }
}
