import { Component, DestroyRef, inject, signal } from '@angular/core';
import { TransactionService } from '../../services/transaction';
import { FormsModule } from '@angular/forms';
import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-deposit',
  imports: [FormsModule, DecimalPipe, RouterLink],
  templateUrl: './deposit.html',
  styleUrl: './deposit.css',
})
export class Deposit {
  private transactionService = inject(TransactionService);
  private destroyRef = inject(DestroyRef);

  balance = this.transactionService.balance;
  isBalanceLoading = this.transactionService.isBalanceLoading;
  balanceError = this.transactionService.balanceError;

  amount: number | null = null;
  isSubmitting = signal(false);
  depositSuccess = signal(false);
  depositError = signal<string | null>(null);

  ngOnInit(): void {
    this.loadBalance();
  }

  loadBalance() {
    const subscription = this.transactionService.getBalance().subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onSubmit() {
    if (!this.amount || this.amount <= 0) return;

    this.isSubmitting.set(true);
    this.depositError.set(null);

    const subscription = this.transactionService.deposit({ amount: this.amount }).subscribe({
      next: () => {
        this.depositSuccess.set(true);
        this.isSubmitting.set(false);
      },
      error: (err) => {
        this.depositError.set(err.message || 'Deposit failed. Please try again.');
        this.isSubmitting.set(false);
      },
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  resetForm() {
    this.amount = null;
    this.depositSuccess.set(false);
    this.depositError.set(null);
  }
}
