import { Component, DestroyRef, inject, signal } from '@angular/core';
import { TransactionService } from '../../services/transaction';
import { FormsModule } from '@angular/forms';
import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BalanceCard } from "../../../../shared/components/balance-card/balance-card";

@Component({
  selector: 'app-withdraw',
  imports: [FormsModule, DecimalPipe, RouterLink, BalanceCard],
  templateUrl: './withdraw.html',
  styleUrl: './withdraw.css',
})
export class Withdraw {
  private transactionService = inject(TransactionService);
  private destroyRef = inject(DestroyRef);

  balance = this.transactionService.balance;
  isBalanceLoading = this.transactionService.isBalanceLoading;
  balanceError = this.transactionService.balanceError;

  amount: number | null = null;
  isSubmitting = signal(false);
  withdrawSuccess = signal(false);
  withdrawError = signal<string | null>(null);

  ngOnInit(): void {
    this.loadBalance();
  }

  loadBalance() {
    const subscription = this.transactionService.getBalance().subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onSubmit() {
    if (!this.amount || this.amount < 5 || this.amount > 8000) return;

    this.isSubmitting.set(true);
    this.withdrawError.set(null);

    const subscription = this.transactionService.withdraw({ amount: this.amount }).subscribe({
      next: () => {
        this.withdrawSuccess.set(true);
        this.isSubmitting.set(false);
      },
      error: (err) => {
        this.withdrawError.set(err.error?.message || err.message || 'Withdrawal failed. Please try again.');
        this.isSubmitting.set(false);
      },
    });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  resetForm() {
    this.amount = null;
    this.withdrawSuccess.set(false);
    this.withdrawError.set(null);
  }
}
