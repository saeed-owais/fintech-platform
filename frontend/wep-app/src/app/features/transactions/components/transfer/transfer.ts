import { Component, DestroyRef, inject, signal } from '@angular/core';
import { TransactionService } from '../../services/transaction';
import { FormsModule } from '@angular/forms';
import { DecimalPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { BalanceCard } from '../../../../shared/components/balance-card/balance-card';

@Component({
  selector: 'app-transfer',
  imports: [FormsModule, DecimalPipe, RouterLink, BalanceCard],
  templateUrl: './transfer.html',
  styleUrl: './transfer.css',
})
export class Transfer {
  private transactionService = inject(TransactionService);
  private destroyRef = inject(DestroyRef);

  balance = this.transactionService.balance;
  isBalanceLoading = this.transactionService.isBalanceLoading;
  balanceError = this.transactionService.balanceError;

  amount: number | null = null;
  receiverEmail: string = '';
  isSubmitting = signal(false);
  transferSuccess = signal(false);
  transferError = signal<string | null>(null);

  ngOnInit(): void {
    this.loadBalance();
  }

  loadBalance() {
    const subscription = this.transactionService.getBalance().subscribe();
    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  onSubmit() {
    if (!this.amount || this.amount < 5 || this.amount > 8000 || !this.receiverEmail) return;

    this.isSubmitting.set(true);
    this.transferError.set(null);

    const subscription = this.transactionService
      .transfer({
        amount: this.amount,
        receiverEmail: this.receiverEmail,
      })
      .subscribe({
        next: () => {
          this.transferSuccess.set(true);
          this.isSubmitting.set(false);
        },
        error: (err) => {
          console.log(err);
          this.transferError.set(err.error?.detail || err.message || 'Transfer failed. Please try again.');
          this.isSubmitting.set(false);
        },
      });

    this.destroyRef.onDestroy(() => subscription.unsubscribe());
  }

  resetForm() {
    this.amount = null;
    this.receiverEmail = '';
    this.transferSuccess.set(false);
    this.transferError.set(null);
  }
}
