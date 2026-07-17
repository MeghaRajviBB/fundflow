import { Routes } from '@angular/router';
import { Landing } from './pages/landing/landing';
import { Shell } from './layout/shell';
import { Dashboard } from './pages/dashboard/dashboard';
import { JournalEntries } from './pages/journal-entries/journal-entries';
import { Treasury } from './pages/treasury/treasury';
import { AccountsPayable } from './pages/accounts-payable/accounts-payable';
import { Grants } from './pages/grants/grants';
import { Budgeting } from './pages/budgeting/budgeting';
import { AnomalyDetection } from './pages/anomaly-detection/anomaly-detection';

export const routes: Routes = [
  { path: '', component: Landing },
  {
    path: 'app',
    component: Shell,
    children: [
      { path: '', component: Dashboard },
      { path: 'journal-entries', component: JournalEntries },
      { path: 'treasury', component: Treasury },
      { path: 'accounts-payable', component: AccountsPayable },
      { path: 'grants', component: Grants },
      { path: 'budgeting', component: Budgeting },
      { path: 'anomaly-detection', component: AnomalyDetection }
    ]
  },
  { path: '**', redirectTo: '' }
];
