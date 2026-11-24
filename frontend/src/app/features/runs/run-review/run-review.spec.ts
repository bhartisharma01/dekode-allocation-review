import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RunReview } from './run-review';

describe('RunReview', () => {
  let component: RunReview;
  let fixture: ComponentFixture<RunReview>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RunReview],
    }).compileComponents();

    fixture = TestBed.createComponent(RunReview);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
