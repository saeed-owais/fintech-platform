import { Component, DestroyRef, inject, signal } from '@angular/core';
import { TransactionService } from '../../services/transaction';
import { TransactionTable } from '../../ui/transaction-table/transaction-table';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-history',
  imports: [FormsModule, RouterLink, TransactionTable],
  templateUrl: './history.html',
  styleUrl: './history.css',
})
export class History {
  private transactionService = inject(TransactionService);
  private destroyRef = inject(DestroyRef);

  history = this.transactionService.history;
  isHistoryLoading = this.transactionService.isHistoryLoading;
  historyError = this.transactionService.historyError;

  selectedType: string = '';
  fromDate: string = '';
  toDate: string = '';
  currentPage = signal(1);
  pageSize = 10;

  ngOnInit(): void {
    this.loadHistory();
  }

  loadHistory() {
    const subscription = this.transactionService
      .getHistory({
        transactionType: this.selectedType as any || undefined,
        fromDate: this.fromDate || undefined,
        toDate: this.toDate || undefined,
        pageNumber: this.currentPage(),
        pageSize: this.pageSize,
      })
      .subscribe();

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onFilterChange() {
    this.currentPage.set(1);
    this.loadHistory();
  }

  clearFilters() {
    this.selectedType = '';
    this.fromDate = '';
    this.toDate = '';
    this.onFilterChange();
  }

  onPageChange(page: number) {
    this.currentPage.set(page);
    this.loadHistory();
  }
}
