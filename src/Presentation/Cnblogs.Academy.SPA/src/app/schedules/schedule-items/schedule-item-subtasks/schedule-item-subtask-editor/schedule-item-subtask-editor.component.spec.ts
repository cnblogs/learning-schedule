import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemSubtaskEditorComponent } from './schedule-item-subtask-editor.component';

describe('ScheduleItemSubtaskEditorComponent', () => {
  let component: ScheduleItemSubtaskEditorComponent;
  let fixture: ComponentFixture<ScheduleItemSubtaskEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemSubtaskEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemSubtaskEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
