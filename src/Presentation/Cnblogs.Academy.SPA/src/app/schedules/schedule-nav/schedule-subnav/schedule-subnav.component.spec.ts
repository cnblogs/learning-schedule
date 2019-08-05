import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleSubnavComponent } from './schedule-subnav.component';

describe('ScheduleSubnavComponent', () => {
  let component: ScheduleSubnavComponent;
  let fixture: ComponentFixture<ScheduleSubnavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleSubnavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleSubnavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
