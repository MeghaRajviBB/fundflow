import { Component, HostListener, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './shell.html',
  styleUrl: './shell.css'
})
export class Shell {
  sidebarOpen = false;

  constructor(private cdr: ChangeDetectorRef) {}

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
