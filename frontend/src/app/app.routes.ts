import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth/auth-guard';
import { adminGuard } from './core/guards/admin/admin-guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login')
        .then(m => m.Login)
  },
  {
    path: 'access-denied',
    loadComponent: () =>
      import('./features/access-denied/access-denied')
        .then(m => m.AccessDenied)
  },
  {
    path: 'runs',
    canActivate: [authGuard, adminGuard],
    loadComponent: () =>
      import('./features/runs/run-history/run-history')
        .then(m => m.RunHistory)
  },
  {
    path: 'runs/:runIdentifier',
    canActivate: [authGuard, adminGuard],
    loadComponent: () =>
      import('./features/runs/run-review/run-review')
        .then(m => m.RunReview)
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];