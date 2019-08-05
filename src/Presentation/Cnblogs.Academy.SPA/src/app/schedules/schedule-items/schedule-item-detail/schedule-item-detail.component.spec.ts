import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemDetailComponent } from './schedule-item-detail.component';

describe('ScheduleItemDetailComponent', () => {
  let component: ScheduleItemDetailComponent;
  let fixture: ComponentFixture<ScheduleItemDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
