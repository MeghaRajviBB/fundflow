export type Role = 'Admin' | 'Accountant' | 'Viewer';

/**
 * Role-based access control for the /app modules.
 *
 * Keyed by each /app child route's path segment ('' = the Dashboard at /app).
 * This is the SINGLE SOURCE OF TRUTH shared by:
 *   - the role guard (guards/role-guard.ts) — restricts direct URL access
 *   - the sidebar (layout/shell.ts) — hides links the role can't access
 * Update permissions here and both stay in sync.
 */
export const MODULE_ROLES: Record<string, Role[]> = {
  '': ['Admin', 'Accountant', 'Viewer'],                // Dashboard
  'journal-entries': ['Admin', 'Accountant', 'Viewer'], // General Ledger
  'treasury': ['Admin', 'Accountant'],
  'accounts-payable': ['Admin', 'Accountant'],
  'grants': ['Admin'],
  'budgeting': ['Admin'],
  'anomaly-detection': ['Admin'],
};

/** True if the given role may access the module identified by `roleKey`. */
export function canRoleAccess(roleKey: string, role: string | null): boolean {
  const allowed = MODULE_ROLES[roleKey];
  if (!allowed) {
    return true; // unknown / unrestricted route → allow
  }
  return role != null && allowed.includes(role as Role);
}
