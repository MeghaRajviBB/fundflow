import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../services/api';

@Component({
  selector: 'app-journal-entries',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './journal-entries.html',
  styleUrl: './journal-entries.css'
})
export class JournalEntries implements OnInit {
  journalEntries: any[] = [];
  loading = true;

  showForm = false;
  editingId: number | null = null;
  confirmingDeleteId: number | null = null;
  funds = ['Operations Fund', 'General Fund', 'Education Fund', 'Grants Account', 'Restricted Fund'];

  entry = this.blankEntry();

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadEntries();
  }

  blankEntry() {
    return {
      description: '',
      debitAmount: 0,
      creditAmount: 0,
      date: new Date().toISOString(),
      fund: 'Operations Fund'
    };
  }

  loadEntries() {
    this.loading = true;
    this.api.getJournalEntries().subscribe({
      next: (data) => {
        this.journalEntries = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => { this.loading = false; this.cdr.detectChanges(); }
    });
  }

  openAddForm() {
    this.editingId = null;
    this.entry = this.blankEntry();
    this.showForm = true;
  }

  openEditForm(e: any) {
    this.editingId = e.id;
    this.entry = { description: e.description, debitAmount: e.debitAmount, creditAmount: e.creditAmount, date: e.date, fund: e.fund };
    this.showForm = true;
  }

  cancelForm() {
    this.showForm = false;
    this.editingId = null;
  }

  save() {
    if (this.editingId) {
      this.api.updateJournalEntry(this.editingId, this.entry).subscribe({
        next: () => { this.showForm = false; this.loadEntries(); },
        error: (err) => alert('Error: ' + (err.error || 'could not update'))
      });
    } else {
      this.api.createJournalEntry(this.entry).subscribe({
        next: () => { this.showForm = false; this.loadEntries(); },
        error: (err) => alert('Error: ' + (err.error || 'could not create'))
      });
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
    this.api.deleteJournalEntry(this.confirmingDeleteId).subscribe({
      next: () => { this.confirmingDeleteId = null; this.loadEntries(); },
      error: () => { this.confirmingDeleteId = null; this.cdr.detectChanges(); alert('Could not delete'); }
    });
  }
}