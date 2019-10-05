import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LinkEditComponent } from './link-edit.component';

describe('LinkEditComponent', () => {
  let component: LinkEditComponent;
  let fixture: ComponentFixture<LinkEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LinkEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LinkEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
