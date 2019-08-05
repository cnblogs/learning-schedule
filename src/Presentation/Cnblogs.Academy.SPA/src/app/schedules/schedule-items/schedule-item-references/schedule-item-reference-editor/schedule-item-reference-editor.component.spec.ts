import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemReferenceEditorComponent } from './schedule-item-reference-editor.component';

describe('ScheduleItemReferenceEditorComponent', () => {
  let component: ScheduleItemReferenceEditorComponent;
  let fixture: ComponentFixture<ScheduleItemReferenceEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemReferenceEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemReferenceEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
