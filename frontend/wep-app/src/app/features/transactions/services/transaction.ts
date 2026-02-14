import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../../environments/environment';
import {
  BalanceResponse,
  DepositRequest,
  DepositResponse,
  HistoryRequest,
  HistoryResponse,
  TransferRequest,
  TransferResponse,
  WithdrawRequest,
  WithdrawResponse,
} from '../models/transaction.model';
import { catchError, finalize, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private readonly baseUrl = environment.apiUrl;

  private _balance = signal<BalanceResponse | null>(null);
  balance = this._balance.asReadonly();
  isBalanceLoading = signal(true);
  balanceError = signal<string | null>(null);

  private _history = signal<HistoryResponse | null>(null);
  history = this._history.asReadonly();
  isHistoryLoading = signal(true);
  historyError = signal<string | null>(null);

  private httpClient = inject(HttpClient);

  getBalance() {
    this.isBalanceLoading.set(true);
    this.balanceError.set(null);

    return this.httpClient.get<BalanceResponse>(`${this.baseUrl}/Transaction/balance`).pipe(
      tap((balance) => this._balance.set(balance)),
      catchError((error) => {
        this.balanceError.set(error.message);
        return of(null);
      }),
      finalize(() => this.isBalanceLoading.set(false)),
    );
  }

  getHistory(options: HistoryRequest) {
    this.isHistoryLoading.set(true);
    this.historyError.set(null);

    const params = new HttpParams()
      .set('transactionType', options.transactionType || '')
      .set('fromDate', options.fromDate || '')
      .set('toDate', options.toDate || '')
      .set('pageNumber', options.pageNumber.toString())
      .set('pageSize', options.pageSize.toString());

    return this.httpClient
      .get<HistoryResponse>(`${this.baseUrl}/Transaction/history`, { params })
      .pipe(
        tap((history) => this._history.set(history)),
        catchError((error) => {
          this.historyError.set(error.error.errors.ToDate[0] || error.message || 'Failed to load history');
          return of(null);
        }),
        finalize(() => this.isHistoryLoading.set(false)),
      );
  }

  deposit(data: DepositRequest) {
    return this.httpClient.post<DepositResponse>(`${this.baseUrl}/Transaction/deposit`, data).pipe(
      tap((response) => {
        const current = this._balance();
        if (current) this._balance.set({ ...current, balance: response.newBalance });
      }),
    );
  }

  withdraw(data: WithdrawRequest) {
    return this.httpClient
      .post<WithdrawResponse>(`${this.baseUrl}/Transaction/withdraw`, data)
      .pipe(
        tap((response) => {
          const current = this._balance();
          if (current) this._balance.set({ ...current, balance: response.newBalance });
        }),
      );
  }

  transfer(data: TransferRequest) {
    return this.httpClient
      .post<TransferResponse>(`${this.baseUrl}/Transaction/transfer`, data)
      .pipe(
        tap((response) => {
          const current = this._balance();
          if (current) this._balance.set({ ...current, balance: response.newBalance });
        }),
      );
  }
}
