import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemSubtasksComponent } from './schedule-item-subtasks.component';

describe('ScheduleItemSubtasksComponent', () => {
  let component: ScheduleItemSubtasksComponent;
  let fixture: ComponentFixture<ScheduleItemSubtasksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemSubtasksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemSubtasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
