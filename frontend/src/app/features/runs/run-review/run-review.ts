
import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RunService, RunReview as RunReviewModel } from '../../../core/services/run/run';
@Component({
  selector: 'app-run-review',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './run-review.html',
  styleUrl: './run-review.scss'
})
export class RunReview implements OnInit {

  runIdentifier: string | null = null;

  run = signal<RunReviewModel | null>(null);
  loading = signal(false);
  errorMessage = signal('');
  exceptionsOnly = signal(false);


  private route = inject(ActivatedRoute);
  private runService = inject(RunService);

  ngOnInit(): void {
    this.runIdentifier =
      this.route.snapshot.paramMap.get('runIdentifier');

    if (!this.runIdentifier) {
      this.errorMessage.set('Invalid run identifier.');
      return;
    }

    this.loadRunReview();
  }

  loadRunReview(): void {
  if (!this.runIdentifier) return;

  this.loading.set(true);
  this.errorMessage.set('');

  this.runService
    .getRunReview(this.runIdentifier, this.exceptionsOnly())
    .subscribe({
      next: (response: any) => {
        this.run.set(response);
        this.loading.set(false);
      },
      error: (error: any) => {
        this.loading.set(false);

        if (error.status === 401) {
          this.errorMessage.set(
            'Your session has expired. Please sign in again.'
          );
        } else if (error.status === 403) {
          this.errorMessage.set(
            'You do not have permission to review this run.'
          );
        } else if (error.status === 404) {
          this.errorMessage.set('Run not found.');
        } else {
          this.errorMessage.set(
            'Unable to load run details. Please try again.'
          );
        }
      }
    });
}
}
