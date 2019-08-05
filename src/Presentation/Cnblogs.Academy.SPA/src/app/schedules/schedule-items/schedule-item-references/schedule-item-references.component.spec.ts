import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleItemReferencesComponent } from './schedule-item-references.component';

describe('ScheduleItemReferencesComponent', () => {
  let component: ScheduleItemReferencesComponent;
  let fixture: ComponentFixture<ScheduleItemReferencesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleItemReferencesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleItemReferencesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
