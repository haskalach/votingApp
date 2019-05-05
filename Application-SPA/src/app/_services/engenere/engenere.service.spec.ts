/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { EngenereService } from './engenere.service';

describe('Service: Engenere', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EngenereService]
    });
  });

  it('should ...', inject([EngenereService], (service: EngenereService) => {
    expect(service).toBeTruthy();
  }));
});
