
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth';
import {
  RunService,
  RunHistoryItem
} from '../../../core/services/run/run';

@Component({
  selector: 'app-run-history',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './run-history.html',
  styleUrl: './run-history.scss'
})
export class RunHistory implements OnInit {
  runs = signal<RunHistoryItem[]>([]);
  loading = signal(false);
  errorMessage = signal('');

  page = signal(1);
  pageSize = 10;
  totalCount = signal(0);
  totalPages = signal(0);

  startRecord = computed(() =>
    this.totalCount() === 0 ? 0 : (this.page() - 1) * this.pageSize + 1
  );
  endRecord = computed(() =>
    Math.min(this.page() * this.pageSize, this.totalCount())
  );

  private runService = inject(RunService);
  private router = inject(Router);
  private authService = inject(AuthService);

  ngOnInit() { this.loadRuns(); }

  loadRuns() {
    this.loading.set(true);
    this.errorMessage.set('');

    this.runService.getRunHistory(this.page(), this.pageSize).subscribe({
      next: (response: any) => {
        this.runs.set(response.items);
        this.totalCount.set(response.totalCount);
        this.totalPages.set(response.totalPages);
        this.loading.set(false);
      },
      error: (error: any) => {
        this.loading.set(false);
        if (error.status === 401) {
          this.errorMessage.set('Your session has expired. Please sign in again.');
        } else if (error.status === 403) {
          this.errorMessage.set('You do not have permission to view run history.');
        } else {
          this.errorMessage.set('Unable to load run history. Please try again.');
        }
      }
    });
  }

  previousPage() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
      this.loadRuns();
    }
  }

  nextPage() {
    if (this.page() < this.totalPages()) {
      this.page.update(p => p + 1);
      this.loadRuns();
    }
  }

  retry() { this.loadRuns(); }
  
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

