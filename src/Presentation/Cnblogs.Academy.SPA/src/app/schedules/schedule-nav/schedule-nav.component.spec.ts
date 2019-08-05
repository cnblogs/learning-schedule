import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleNavComponent } from './schedule-nav.component';

describe('ScheduleNavComponent', () => {
  let component: ScheduleNavComponent;
  let fixture: ComponentFixture<ScheduleNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
