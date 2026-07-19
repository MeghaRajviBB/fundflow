import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Auth } from '../services/auth';
import { Role } from '../auth/roles';

/**
 * Restricts a route to the roles listed in its `data.roles`.
 * A logged-in user whose role isn't allowed is sent back to the dashboard (/app).
 * (The parent /app route's authGuard already handles the not-logged-in case.)
 */
export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const auth = inject(Auth);
  const router = inject(Router);

  const allowed = route.data['roles'] as Role[] | undefined;

  // No role restriction declared on this route → allow.
  if (!allowed || allowed.length === 0) {
    return true;
  }

  const role = auth.getRole() as Role | null;
  if (role && allowed.includes(role)) {
    return true;
  }

  router.navigate(['/app']);
  return false;
};
