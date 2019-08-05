import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleIndexComponent } from './schedule-index.component';

describe('ScheduleIndexComponent', () => {
  let component: ScheduleIndexComponent;
  let fixture: ComponentFixture<ScheduleIndexComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleIndexComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleIndexComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
