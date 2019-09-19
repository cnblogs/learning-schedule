import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SchedulesComponent } from './schedules.component';
import { RouterModule, Routes } from '@angular/router';
import { ScheduleIndexComponent } from './schedule-index/schedule-index.component';
import { ToolsModule } from '../shared/tools/tools.module';
import { SharedModule } from '../shared/shared.module';
import { MarkdownEditorModule } from '../shared/markdown-editor/markdown-editor.module';
import { ScheduleItemsComponent } from './schedule-items/schedule-items.component';
import { ScheduleHeaderEditorComponent } from './schedule-header-editor/schedule-header-editor.component';
import { ScheduleItemEditorComponent } from './schedule-items/schedule-item-editor/schedule-item-editor.component';
import { ModalDirective } from './modal/modal.directive';
import { ModalOpenDirective } from './modal/modal-open.directive';
import { BoardDirective } from './modal/board.directive';
import { PanMenuComponent } from './pan-menu/pan-menu.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { FormsModule } from '@angular/forms';
import { AuthGuardService } from '../auth-guard.service';
import { ScheduleDetailComponent } from './schedule-detail/schedule-detail.component';
import { ScheduleListComponent } from './schedule-list/schedule-list.component';
import { ScheduleSubnavComponent } from './schedule-subnav/schedule-subnav.component';
import { SwitcherService } from './switcher.service';
import { ScheduleItemTitleComponent } from './schedule-items/schedule-item-title/schedule-item-title.component';
import { SummaryComponent } from './schedule-items/summary/summary.component';
import { NoteEditComponent } from './schedule-items/summary/note-edit/note-edit.component';
import { LinkEditComponent } from './schedule-items/summary/link-edit/link-edit.component';
import { NoteComponent } from './schedule-items/summary/note/note.component';
import { LinkComponent } from './schedule-items/summary/link/link.component';

const routes: Routes = [
  {
    path: '', component: ScheduleSubnavComponent, children: [
      { path: 'doing', component: ScheduleIndexComponent },
      { path: 'done', component: ScheduleIndexComponent },
      { path: '', pathMatch: 'full', redirectTo: 'doing' }
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ToolsModule,
    SharedModule,
    MarkdownEditorModule,
    RouterModule.forChild([
      { path: 'u/:alias/:scheduleId', component: ScheduleDetailComponent },
      { path: 'u/:alias/:scheduleId/item/:itemId/details', redirectTo: 'u/:alias/:scheduleId' },
      { path: 'u/:alias/:scheduleId/item/:itemId', redirectTo: 'u/:alias/:scheduleId' },
      { path: 'u/:alias/:scheduleId/detail/:itemId', redirectTo: 'u/:alias/:scheduleId' },
      {
        path: 'target', children: routes, data: { target: true }
      },
      ...routes
    ])
  ],
  declarations: [
    SchedulesComponent,
    ScheduleIndexComponent,
    ScheduleItemsComponent,
    ScheduleHeaderEditorComponent,
    ScheduleItemEditorComponent,
    ModalDirective,
    ModalOpenDirective,
    BoardDirective,
    PanMenuComponent,
    ScheduleComponent,
    ScheduleDetailComponent,
    ScheduleListComponent,
    ScheduleSubnavComponent,
    ScheduleItemTitleComponent,
    SummaryComponent,
    NoteEditComponent,
    LinkEditComponent,
    NoteComponent,
    LinkComponent,
  ],
  providers: [
    SwitcherService
  ],
  exports: [
    ScheduleItemTitleComponent,
    ScheduleItemEditorComponent,
    ScheduleHeaderEditorComponent
  ]
})
export class SchedulesModule { }
