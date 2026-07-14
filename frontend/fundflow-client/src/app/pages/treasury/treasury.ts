import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../services/api';

@Component({
  selector: 'app-treasury',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './treasury.html',
  styleUrl: './treasury.css'
})
export class Treasury implements OnInit {
  transactions: any[] = [];
  loading = true;

  showForm = false;
  editingId: number | null = null;
  types = ['Credit', 'Debit'];
  accounts = ['Main Account', 'Grants Account', 'Reserve Account'];

  selectedIds: number[] = [];
  clearing = false;

  txn = this.blank();

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.load(); }

  blank() {
    return { description: '', amount: 0, date: new Date().toISOString(), type: 'Credit', isCleared: false, bankAccount: 'Main Account' };
  }

  load() {
    this.loading = true;
    this.selectedIds = [];
    this.api.getBankTransactions().subscribe({
      next: (data) => { this.transactions = data; this.loading = false; this.cdr.detectChanges(); },
      error: () => { this.loading = false; this.cdr.detectChanges(); }
    });
  }

  toggleSelect(id: number) {
    if (this.selectedIds.includes(id)) {
      this.selectedIds = this.selectedIds.filter(x => x !== id);
    } else {
      this.selectedIds = [...this.selectedIds, id];
    }
  }

  isSelected(id: number) {
    return this.selectedIds.includes(id);
  }

  get pendingCount() {
    return this.transactions.filter(t => !t.isCleared).length;
  }

  clearSelected() {
    if (this.selectedIds.length === 0) return;
    this.clearing = true;
    this.api.bulkClear(this.selectedIds).subscribe({
      next: () => { this.clearing = false; this.load(); },
      error: () => { this.clearing = false; }
    });
  }

  openAdd() { this.editingId = null; this.txn = this.blank(); this.showForm = true; }

  openEdit(t: any) {
    this.editingId = t.id;
    this.txn = { description: t.description, amount: t.amount, date: t.date, type: t.type, isCleared: t.isCleared, bankAccount: t.bankAccount };
    this.showForm = true;
  }

  cancel() { this.showForm = false; this.editingId = null; }

  save() {
    if (this.editingId) {
      this.api.updateTransaction(this.editingId, this.txn).subscribe({ next: () => { this.showForm = false; this.load(); } });
    } else {
      this.api.createTransaction(this.txn).subscribe({ next: () => { this.showForm = false; this.load(); } });
    }
  }

  remove(id: number) {
    if (!confirm('Delete this transaction?')) return;
    this.api.deleteTransaction(id).subscribe({ next: () => this.load() });
  }
}