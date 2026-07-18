import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../services/api';

@Component({
  selector: 'app-grants',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './grants.html',
  styleUrl: './grants.css'
})
export class Grants implements OnInit {
  grants: any[] = [];
  loading = true;

  showForm = false;
  editingId: number | null = null;
  confirmingDeleteId: number | null = null;
  statuses = ['Active', 'Closed', 'Pending'];

  grant = this.blank();

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.load(); }

  blank() {
    return { grantName: '', funderName: '', totalAmount: 0, spentAmount: 0, startDate: new Date().toISOString(), endDate: new Date().toISOString(), status: 'Active' };
  }

  load() {
    this.loading = true;
    this.api.getGrants().subscribe({
      next: (data) => { this.grants = data; this.loading = false; this.cdr.detectChanges(); },
      error: () => { this.loading = false; this.cdr.detectChanges(); }
    });
  }

  openAdd() { this.editingId = null; this.grant = this.blank(); this.showForm = true; }

  openEdit(g: any) {
    this.editingId = g.id;
    this.grant = { grantName: g.grantName, funderName: g.funderName, totalAmount: g.totalAmount, spentAmount: g.spentAmount, startDate: g.startDate, endDate: g.endDate, status: g.status };
    this.showForm = true;
  }

  cancel() { this.showForm = false; this.editingId = null; }

  save() {
    if (this.editingId) {
      this.api.updateGrant(this.editingId, this.grant).subscribe({ next: () => { this.showForm = false; this.load(); } });
    } else {
      this.api.createGrant(this.grant).subscribe({ next: () => { this.showForm = false; this.load(); } });
    }
  }

  askDelete(id: number) {
    this.confirmingDeleteId = id;
  }

  cancelDelete() {
    this.confirmingDeleteId = null;
  }

  confirmDelete() {
    if (this.confirmingDeleteId === null) return;
    this.api.deleteGrant(this.confirmingDeleteId).subscribe({
      next: () => { this.confirmingDeleteId = null; this.load(); },
      error: () => { this.confirmingDeleteId = null; this.cdr.detectChanges(); }
    });
  }
}