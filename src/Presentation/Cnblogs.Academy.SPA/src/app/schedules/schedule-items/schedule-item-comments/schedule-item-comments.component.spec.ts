import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemCommentsComponent } from './schedule-item-comments.component';

describe('ScheduleItemCommentsComponent', () => {
  let component: ScheduleItemCommentsComponent;
  let fixture: ComponentFixture<ScheduleItemCommentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemCommentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
