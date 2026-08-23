import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = () => {
  const router = inject(Router);

  const token = localStorage.getItem('token');
  const expiresAt = localStorage.getItem('tokenExpiresAt');

  if (!token || !expiresAt) {
    return router.createUrlTree(['/login']);
  }

  const expired = new Date(expiresAt) <= new Date();

  if (expired) {
    localStorage.removeItem('token');
    localStorage.removeItem('tokenExpiresAt');

    return router.createUrlTree(['/login']);
  }

  return true;
};
