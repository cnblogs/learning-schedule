import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemEditorComponent } from './schedule-item-editor.component';

describe('ScheduleItemEditorComponent', () => {
  let component: ScheduleItemEditorComponent;
  let fixture: ComponentFixture<ScheduleItemEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
