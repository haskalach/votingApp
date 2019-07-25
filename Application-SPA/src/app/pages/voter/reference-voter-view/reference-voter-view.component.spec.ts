import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReferenceVoterViewComponent } from './reference-voter-view.component';

describe('ReferenceVoterViewComponent', () => {
  let component: ReferenceVoterViewComponent;
  let fixture: ComponentFixture<ReferenceVoterViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReferenceVoterViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReferenceVoterViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
