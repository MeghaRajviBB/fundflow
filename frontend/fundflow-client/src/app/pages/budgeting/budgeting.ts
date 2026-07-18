import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../services/api';

@Component({
  selector: 'app-budgeting',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './budgeting.html',
  styleUrl: './budgeting.css'
})
export class Budgeting implements OnInit {
  budgets: any[] = [];
  loading = true;

  showForm = false;
  editingId: number | null = null;
  confirmingDeleteId: number | null = null;
  quarters = ['Q1', 'Q2', 'Q3', 'Q4'];

  budget = this.blank();

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() { this.load(); }

  blank() {
    return { department: '', category: '', budgetedAmount: 0, actualAmount: 0, fiscalYear: 2026, quarter: 'Q1' };
  }

  load() {
    this.loading = true;
    this.api.getBudgets().subscribe({
      next: (data) => { this.budgets = data; this.loading = false; this.cdr.detectChanges(); },
      error: () => { this.loading = false; this.cdr.detectChanges(); }
    });
  }

  openAdd() { this.editingId = null; this.budget = this.blank(); this.showForm = true; }

  openEdit(b: any) {
    this.editingId = b.id;
    this.budget = { department: b.department, category: b.category, budgetedAmount: b.budgetedAmount, actualAmount: b.actualAmount, fiscalYear: b.fiscalYear, quarter: b.quarter };
    this.showForm = true;
  }

  cancel() { this.showForm = false; this.editingId = null; }

  save() {
    if (this.editingId) {
      this.api.updateBudget(this.editingId, this.budget).subscribe({ next: () => { this.showForm = false; this.load(); } });
    } else {
      this.api.createBudget(this.budget).subscribe({ next: () => { this.showForm = false; this.load(); } });
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
    this.api.deleteBudget(this.confirmingDeleteId).subscribe({
      next: () => { this.confirmingDeleteId = null; this.load(); },
      error: () => { this.confirmingDeleteId = null; this.cdr.detectChanges(); }
    });
  }
}