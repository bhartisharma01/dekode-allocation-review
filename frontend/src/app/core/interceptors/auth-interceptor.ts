
import {
  HttpInterceptorFn,
  HttpErrorResponse
} from '@angular/common/http';

import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../services/auth/auth';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();

  // Don't attach an old token to the login request.
  const isLoginRequest = req.url.endsWith('/api/auth/login');

  const authRequest = token && !isLoginRequest
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      })
    : req;

  return next(authRequest).pipe(
    catchError((error: HttpErrorResponse) => {

      // Handle expired or invalid authentication tokens.
      if (error.status === 401 && !isLoginRequest) {
        authService.logout();

        router.navigate(['/login']);
      }

      return throwError(() => error);
    })
  );
};
