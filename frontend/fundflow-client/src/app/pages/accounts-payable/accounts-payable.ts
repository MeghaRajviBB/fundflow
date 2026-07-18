import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../services/api';

@Component({
  selector: 'app-accounts-payable',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './accounts-payable.html',
  styleUrl: './accounts-payable.css'
})
export class AccountsPayable implements OnInit {
  invoices: any[] = [];
  loading = true;

  showForm = false;
  editingId: number | null = null;
  confirmingDeleteId: number | null = null;
  statuses = ['Pending', 'Approved', 'Paid'];

  inv = this.blank();

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.load(); }

  blank() {
    return { vendorName: '', description: '', amount: 0, invoiceDate: new Date().toISOString(), dueDate: new Date().toISOString(), status: 'Pending' };
  }

  load() {
    this.loading = true;
    this.api.getInvoices().subscribe({
      next: (data) => { this.invoices = data; this.loading = false; this.cdr.detectChanges(); },
      error: () => { this.loading = false; this.cdr.detectChanges(); }
    });
  }

  openAdd() { this.editingId = null; this.inv = this.blank(); this.showForm = true; }

  openEdit(i: any) {
    this.editingId = i.id;
    this.inv = { vendorName: i.vendorName, description: i.description, amount: i.amount, invoiceDate: i.invoiceDate, dueDate: i.dueDate, status: i.status };
    this.showForm = true;
  }

  cancel() { this.showForm = false; this.editingId = null; }

  save() {
    if (this.editingId) {
      this.api.updateInvoice(this.editingId, this.inv).subscribe({ next: () => { this.showForm = false; this.load(); } });
    } else {
      this.api.createInvoice(this.inv).subscribe({ next: () => { this.showForm = false; this.load(); } });
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
    this.api.deleteInvoice(this.confirmingDeleteId).subscribe({
      next: () => { this.confirmingDeleteId = null; this.load(); },
      error: () => { this.confirmingDeleteId = null; this.cdr.detectChanges(); }
    });
  }
}