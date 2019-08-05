import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemFeedbacksComponent } from './schedule-item-feedbacks.component';

describe('ScheduleItemFeedbackComponent', () => {
  let component: ScheduleItemFeedbacksComponent;
  let fixture: ComponentFixture<ScheduleItemFeedbacksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemFeedbacksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemFeedbacksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
