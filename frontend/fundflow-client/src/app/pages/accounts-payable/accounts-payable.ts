import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api } from '../../services/api';

@Component({
  selector: 'app-accounts-payable',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './accounts-payable.html',
  styleUrl: './accounts-payable.css'
})
export class AccountsPayable implements OnInit {
  invoices: any[] = [];
  loading = true;

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.api.getInvoices().subscribe({
      next: (data) => {
        this.invoices = data;
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