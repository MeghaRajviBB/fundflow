import { Routes } from '@angular/router';
import { Landing } from './pages/landing/landing';
import { Login } from './pages/login/login';
import { Shell } from './layout/shell';
import { authGuard } from './guards/auth-guard';
import { roleGuard } from './guards/role-guard';
import { MODULE_ROLES } from './auth/roles';
import { Dashboard } from './pages/dashboard/dashboard';
import { JournalEntries } from './pages/journal-entries/journal-entries';
import { Treasury } from './pages/treasury/treasury';
import { AccountsPayable } from './pages/accounts-payable/accounts-payable';
import { Grants } from './pages/grants/grants';
import { Budgeting } from './pages/budgeting/budgeting';
import { AnomalyDetection } from './pages/anomaly-detection/anomaly-detection';

export const routes: Routes = [
  { path: '', component: Landing },
  { path: 'login', component: Login },
  {
    path: 'app',
    component: Shell,
    canActivate: [authGuard],
    children: [
      { path: '', component: Dashboard, canActivate: [roleGuard], data: { roles: MODULE_ROLES[''] } },
      { path: 'journal-entries', component: JournalEntries, canActivate: [roleGuard], data: { roles: MODULE_ROLES['journal-entries'] } },
      { path: 'treasury', component: Treasury, canActivate: [roleGuard], data: { roles: MODULE_ROLES['treasury'] } },
      { path: 'accounts-payable', component: AccountsPayable, canActivate: [roleGuard], data: { roles: MODULE_ROLES['accounts-payable'] } },
      { path: 'grants', component: Grants, canActivate: [roleGuard], data: { roles: MODULE_ROLES['grants'] } },
      { path: 'budgeting', component: Budgeting, canActivate: [roleGuard], data: { roles: MODULE_ROLES['budgeting'] } },
      { path: 'anomaly-detection', component: AnomalyDetection, canActivate: [roleGuard], data: { roles: MODULE_ROLES['anomaly-detection'] } }
    ]
  },
  { path: '**', redirectTo: '' }
];
