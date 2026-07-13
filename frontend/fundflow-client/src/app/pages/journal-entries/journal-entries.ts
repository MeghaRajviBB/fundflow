import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api } from '../../services/api';

@Component({
  selector: 'app-journal-entries',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './journal-entries.html',
  styleUrl: './journal-entries.css'
})
export class JournalEntries implements OnInit {
  journalEntries: any[] = [];
  loading = true;

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.api.getJournalEntries().subscribe({
      next: (data) => {
        this.journalEntries = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching data:', err);
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}