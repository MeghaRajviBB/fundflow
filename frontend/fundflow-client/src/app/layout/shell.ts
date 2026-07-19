import { Component, HostListener, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { Auth } from '../services/auth';
import { canRoleAccess } from '../auth/roles';

interface NavItem {
  label: string;
  link: string;
  roleKey: string;
  exact?: boolean;
  ai?: boolean;
}

interface NavSection {
  title: string;
  items: NavItem[];
}

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './shell.html',
  styleUrl: './shell.css'
})
export class Shell {
  sidebarOpen = false;

  // Full navigation model; roleKey maps to MODULE_ROLES in auth/roles.ts.
  private readonly navModel: NavSection[] = [
    {
      title: 'Overview',
      items: [
        { label: 'Dashboard', link: '/app', roleKey: '', exact: true }
      ]
    },
    {
      title: 'Modules',
      items: [
        { label: 'General Ledger', link: '/app/journal-entries', roleKey: 'journal-entries' },
        { label: 'Treasury', link: '/app/treasury', roleKey: 'treasury' },
        { label: 'Accounts Payable', link: '/app/accounts-payable', roleKey: 'accounts-payable' },
        { label: 'Grants', link: '/app/grants', roleKey: 'grants' },
        { label: 'Budgeting', link: '/app/budgeting', roleKey: 'budgeting' }
      ]
    },
    {
      title: 'Intelligence',
      items: [
        { label: 'Anomaly Detection', link: '/app/anomaly-detection', roleKey: 'anomaly-detection', ai: true }
      ]
    }
  ];

  constructor(private auth: Auth, private cdr: ChangeDetectorRef) {}

  /** Nav sections filtered to the current user's role; sections with no visible items are dropped. */
  get navSections(): NavSection[] {
    const role = this.auth.getRole();
    return this.navModel
      .map(section => ({
        title: section.title,
        items: section.items.filter(item => canRoleAccess(item.roleKey, role))
      }))
      .filter(section => section.items.length > 0);
  }

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

  closeSidebar() {
    this.sidebarOpen = false;
  }

  @HostListener('document:keydown.escape')
  onEscape() {
    if (this.sidebarOpen) {
      this.sidebarOpen = false;
      this.cdr.detectChanges();
    }
  }
}
