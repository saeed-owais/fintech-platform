import { Component, DestroyRef, inject, signal } from '@angular/core';

import { TransactionService } from '../../services/transaction';
import { TransactionTable } from '../../ui/transaction-table/transaction-table';
import { BalanceCard } from '../../../../shared/components/balance-card/balance-card';
import { QuickActionCard } from '../../../../shared/components/quick-action-card/quick-action-card';
@Component({
  selector: 'app-dashboard',
  imports: [TransactionTable, BalanceCard, QuickActionCard],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard {
  private transactionService = inject(TransactionService);
  private destroyRef = inject(DestroyRef);

  balance = this.transactionService.balance;
  isBalanceLoading = this.transactionService.isBalanceLoading;
  balanceError = this.transactionService.balanceError;

  history = this.transactionService.history;
  isHistoryLoading = this.transactionService.isHistoryLoading;
  historyError = this.transactionService.historyError;

  ngOnInit(): void {
    this.loadBalance();

    this.loadHistory();
  }

  loadBalance() {
    const subscription = this.transactionService.getBalance().subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  loadHistory() {
    const subscription = this.transactionService
      .getHistory({ pageNumber: 1, pageSize: 5 })
      .subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }
}
