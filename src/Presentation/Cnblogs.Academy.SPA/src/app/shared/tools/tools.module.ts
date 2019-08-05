import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CountdownDirective } from './countdown.directive';
import { FocusOnShowDirective } from './focus-on-show.directive';
import { HighlightCodeDirective } from './highlight-code.directive';
import { ExtenalLinksDirective } from './extenal-links.directive';
import { AutosizeDirective } from './autosize.directive';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    CountdownDirective,
    FocusOnShowDirective,
    HighlightCodeDirective,
    ExtenalLinksDirective,
    AutosizeDirective,
  ],
  exports: [
    CountdownDirective,
    FocusOnShowDirective,
    HighlightCodeDirective,
    ExtenalLinksDirective,
    AutosizeDirective
  ]
})
export class ToolsModule { }
