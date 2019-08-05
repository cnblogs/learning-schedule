import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownEditorComponent } from './markdown-editor.component';
import { FormsModule } from '@angular/forms';
import { ToolsModule } from '../tools/tools.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ToolsModule
  ],
  declarations: [MarkdownEditorComponent],
  exports: [MarkdownEditorComponent]
})
export class MarkdownEditorModule { }
