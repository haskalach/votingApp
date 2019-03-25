/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddVoterComponent } from './add-voter.component';

describe('AddVoterComponent', () => {
  let component: AddVoterComponent;
  let fixture: ComponentFixture<AddVoterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddVoterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddVoterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
