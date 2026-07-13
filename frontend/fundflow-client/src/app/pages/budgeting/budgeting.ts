import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api } from '../../services/api';

@Component({
  selector: 'app-budgeting',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './budgeting.html',
  styleUrl: './budgeting.css'
})
export class Budgeting implements OnInit {
  budgets: any[] = [];
  loading = true;

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.api.getBudgets().subscribe({
      next: (data) => {
        this.budgets = data;
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