import { NgModule } from '@angular/core';
import { TinyEditorComponent } from './tiny-editor.component';
import { EditorModule } from 'self-tinymce-angular';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    FormsModule,
    EditorModule
  ],
  declarations: [TinyEditorComponent],
  exports: [TinyEditorComponent]
})
export class TinyEditorModule { }
