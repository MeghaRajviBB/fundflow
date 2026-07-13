import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Api } from '../../services/api';

@Component({
  selector: 'app-grants',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './grants.html',
  styleUrl: './grants.css'
})
export class Grants implements OnInit {
  grants: any[] = [];
  loading = true;

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.api.getGrants().subscribe({
      next: (data) => {
        this.grants = data;
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