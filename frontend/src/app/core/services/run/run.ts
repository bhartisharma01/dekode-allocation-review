
import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment.development';

// One row in the run history table
export interface RunHistoryItem {
  runIdentifier: string;
  runDate: string;
  totalAllocatedUnits: number;
}

// Generic paginated API response
export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

// Possible gap statuses
export type GapStatus = 'Filled' | 'Partially filled' | 'Unfilled';

// One gap in the run review
export interface GapReview {
  gapIdentifier: string;
  store: string;
  brand: string;
  productType: string;
  originalShortfall: number;
  allocatedUnits: number;
  remainingUnits: number;
  status: GapStatus;
}

// Complete run review response
export interface RunReview {
  runIdentifier: string;
  runDate: string;
  totalAllocatedUnits: number;
  totalGaps: number;
  filledCount: number;
  partiallyFilledCount: number;
  unfilledCount: number;
  gaps: GapReview[];
}

@Injectable({
  providedIn: 'root'
})
export class RunService {
  private http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/auth`;


  // Fetch paginated run history
  getRunHistory(
    page: number = 1,
    pageSize: number = 10
  ): Observable<PagedResult<RunHistoryItem>> {

    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedResult<RunHistoryItem>>(
      this.apiUrl,
      { params }
    );
  }

  // Fetch details of a particular run
  getRunReview(
    runIdentifier: string,
    exceptionsOnly: boolean = false
  ): Observable<RunReview> {

    const params = new HttpParams()
      .set('exceptionsOnly', exceptionsOnly);

    return this.http.get<RunReview>(
      `${this.apiUrl}/${encodeURIComponent(runIdentifier)}`,
      { params }
    );
  }
}
