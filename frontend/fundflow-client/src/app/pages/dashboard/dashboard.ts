import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api } from '../../services/api';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  journalCount = 0;
  transactionCount = 0;
  invoiceCount = 0;
  grantCount = 0;
  budgetCount = 0;
  loading = true;

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.api.getJournalEntries().subscribe(d => { this.journalCount = d.length; this.cdr.detectChanges(); });
    this.api.getBankTransactions().subscribe(d => { this.transactionCount = d.length; this.cdr.detectChanges(); });
    this.api.getInvoices().subscribe(d => { this.invoiceCount = d.length; this.cdr.detectChanges(); });
    this.api.getGrants().subscribe(d => { this.grantCount = d.length; this.cdr.detectChanges(); });
    this.api.getBudgets().subscribe(d => { this.budgetCount = d.length; this.loading = false; this.cdr.detectChanges(); });
  }
}