import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemTitleComponent } from './schedule-item-title.component';

describe('ScheduleItemTitleComponent', () => {
  let component: ScheduleItemTitleComponent;
  let fixture: ComponentFixture<ScheduleItemTitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemTitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemTitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
