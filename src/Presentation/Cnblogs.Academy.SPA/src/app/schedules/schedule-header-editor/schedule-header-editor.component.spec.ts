import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleHeaderEditorComponent } from './schedule-header-editor.component';

describe('ScheduleHeaderEditorComponent', () => {
  let component: ScheduleHeaderEditorComponent;
  let fixture: ComponentFixture<ScheduleHeaderEditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleHeaderEditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleHeaderEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
