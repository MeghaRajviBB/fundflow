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
  createJournalEntry(entry: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/JournalEntry`, entry);
  }

  updateJournalEntry(id: number, entry: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/JournalEntry/${id}`, entry);
  }

  deleteJournalEntry(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/JournalEntry/${id}`, { responseType: 'text' });
  }
  createTransaction(t: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Treasury`, t);
  }

  updateTransaction(id: number, t: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/Treasury/${id}`, t);
  }

  deleteTransaction(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Treasury/${id}`, { responseType: 'text' });
  }
  createInvoice(inv: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/AccountsPayable`, inv);
  }

  updateInvoice(id: number, inv: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/AccountsPayable/${id}`, inv);
  }

  deleteInvoice(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/AccountsPayable/${id}`, { responseType: 'text' });
  }
  createGrant(g: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Grants`, g);
  }

  updateGrant(id: number, g: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/Grants/${id}`, g);
  }

  deleteGrant(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Grants/${id}`, { responseType: 'text' });
  }
  createBudget(b: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Budgeting`, b);
  }

  updateBudget(id: number, b: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/Budgeting/${id}`, b);
  }

  deleteBudget(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Budgeting/${id}`, { responseType: 'text' });
  }
  bulkClear(ids: number[]): Observable<any> {
    return this.http.put(`${this.baseUrl}/Treasury/bulkclear`, ids, { responseType: 'text' });
  }
}