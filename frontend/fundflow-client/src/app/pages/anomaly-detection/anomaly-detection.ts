import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../services/api';

@Component({
  selector: 'app-anomaly-detection',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './anomaly-detection.html',
  styleUrl: './anomaly-detection.css'
})
export class AnomalyDetection {
  entry = {
    description: '',
    debitAmount: 0,
    creditAmount: 0,
    date: new Date().toISOString(),
    fund: 'Operations Fund'
  };

  result: any = null;
  analyzing = false;

  scanResult: any = null;
  scanning = false;

  funds = ['Operations Fund', 'General Fund', 'Education Fund', 'Grants Account', 'Restricted Fund'];

  constructor(private api: Api, private cdr: ChangeDetectorRef) {}

  analyze() {
    this.analyzing = true;
    this.result = null;
    this.api.analyzeEntry(this.entry).subscribe({
      next: (res) => {
        this.result = res;
        this.analyzing = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.analyzing = false;
        this.cdr.detectChanges();
      }
    });
  }

  scanAll() {
    this.scanning = true;
    this.scanResult = null;
    this.api.scanAnomalies().subscribe({
      next: (res) => {
        this.scanResult = res;
        this.scanning = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.scanning = false;
        this.cdr.detectChanges();
      }
    });
  }
}