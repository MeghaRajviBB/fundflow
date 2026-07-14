import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class Api {
  private baseUrl = 'https://localhost:7129/api';

  constructor(private http: HttpClient) { }

  getJournalEntries(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/JournalEntry`);
  }

  getBankTransactions(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Treasury`);
  }

  getInvoices(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/AccountsPayable`);
  }

  getGrants(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Grants`);
  }

  getBudgets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Budgeting`);
  }
  analyzeEntry(entry: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/JournalEntry/analyze`, entry);
  }

  scanAnomalies(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/JournalEntry/scan-anomalies`);
  }
}