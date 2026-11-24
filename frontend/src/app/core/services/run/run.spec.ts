import { TestBed } from '@angular/core/testing';

import { Run } from './run';

describe('Run', () => {
  let service: Run;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Run);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
