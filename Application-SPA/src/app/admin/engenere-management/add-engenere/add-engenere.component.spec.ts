/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AddEngenereComponent } from './add-engenere.component';

describe('AddEngenereComponent', () => {
  let component: AddEngenereComponent;
  let fixture: ComponentFixture<AddEngenereComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEngenereComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEngenereComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
