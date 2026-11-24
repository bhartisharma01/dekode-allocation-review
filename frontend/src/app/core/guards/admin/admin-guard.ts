import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth';
import { UserRole } from '../../models/user-roles';

export const adminGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = authService.getToken();
  const role = authService.getRole();

  if (!token) return router.createUrlTree(['/login']);
  

  if (role !== UserRole.ADMIN) {
    return router.createUrlTree(['/access-denied']);
  }

  return true;
};