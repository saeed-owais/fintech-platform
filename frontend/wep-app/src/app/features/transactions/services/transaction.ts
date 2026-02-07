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
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  private readonly baseUrl = environment.apiUrl;

  private _balance = signal<BalanceResponse | null>(null);
  balance = this._balance.asReadonly();

  private httpClient = inject(HttpClient);

  getBalance() {
    return this.httpClient
      .get<BalanceResponse>(`${this.baseUrl}/Transaction/balance`)
      .pipe(tap((balance) => this._balance.set(balance)));
  }

  getHistory(options: HistoryRequest) {
    const params = new HttpParams()
      .set('transactionType', options.transactionType || '')
      .set('fromDate', options.fromDate || '')
      .set('toDate', options.toDate || '')
      .set('pageNumber', options.pageNumber.toString())
      .set('pageSize', options.pageSize.toString());

    return this.httpClient.get<HistoryResponse>(`${this.baseUrl}/Transaction/history`, { params });
  }

  deposit(data: DepositRequest) {
    return this.httpClient
      .post<DepositResponse>(`${this.baseUrl}/Transaction/deposit`, data)
      .pipe(
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
