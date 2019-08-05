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
import { ScheduleNavComponent } from './schedule-nav/schedule-nav.component';
import { ScheduleSubnavComponent } from './schedule-nav/schedule-subnav/schedule-subnav.component';
import { ScheduleItemSubtasksComponent } from './schedule-items/schedule-item-subtasks/schedule-item-subtasks.component';
import { ScheduleItemDetailComponent } from './schedule-items/schedule-item-detail/schedule-item-detail.component';
import { ScheduleItemReferencesComponent } from './schedule-items/schedule-item-references/schedule-item-references.component';
import { ScheduleItemCommentsComponent } from './schedule-items/schedule-item-comments/schedule-item-comments.component';
import { ScheduleItemSubtaskEditorComponent } from './schedule-items/schedule-item-subtasks/schedule-item-subtask-editor/schedule-item-subtask-editor.component';
import { ScheduleItemReferenceEditorComponent } from './schedule-items/schedule-item-references/schedule-item-reference-editor/schedule-item-reference-editor.component';
import { SwitcherService } from './switcher.service';
import { ScheduleItemTitleComponent } from './schedule-items/schedule-item-title/schedule-item-title.component';
import { ScheduleItemFeedbacksComponent } from './schedule-items/schedule-item-feedbacks/schedule-item-feedbacks.component';

const routes: Routes = [
  {
    path: 'study', component: ScheduleSubnavComponent, children: [
      { path: 'doing', component: ScheduleIndexComponent },
      { path: 'done', component: ScheduleIndexComponent },
      { path: '', pathMatch: 'full', redirectTo: 'doing' }
    ]
  },
  {
    path: 'teach', component: ScheduleSubnavComponent, data: { teach: true }, children: [
      { path: 'doing', component: ScheduleIndexComponent },
      { path: 'done', component: ScheduleIndexComponent },
      { path: '', pathMatch: 'full', redirectTo: 'doing' }
    ]
  },
  {
    path: '', pathMatch: 'full', redirectTo: 'study'
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
      {
        path: 'u/:alias/:scheduleId', component: ScheduleDetailComponent, children: [
          { path: 'item/:itemId/details', component: ScheduleItemDetailComponent },
          { path: 'item/:itemId', redirectTo: 'item/:itemId/details' }
        ]
      },
      { path: 'u/:alias/:scheduleId/detail/:itemId', redirectTo: 'u/:alias/:scheduleId/item/:itemId/details' },
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
    ScheduleNavComponent,
    ScheduleSubnavComponent,
    ScheduleItemSubtasksComponent,
    ScheduleItemDetailComponent,
    ScheduleItemReferencesComponent,
    ScheduleItemCommentsComponent,
    ScheduleItemSubtaskEditorComponent,
    ScheduleItemReferenceEditorComponent,
    ScheduleItemTitleComponent,
    ScheduleItemFeedbacksComponent
  ],
  providers: [
    SwitcherService
  ]
})
export class SchedulesModule { }
