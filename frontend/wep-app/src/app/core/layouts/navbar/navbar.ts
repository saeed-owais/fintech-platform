import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../services/auth';
import { TransactionService } from '../../../features/transactions/services/transaction';
import { BalanceCard } from '../../../shared/components/balance-card/balance-card';

@Component({
  selector: 'app-navbar',
  imports: [BalanceCard],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private authService = inject(AuthService);
  private transactionService = inject(TransactionService);

  balance = this.transactionService.balance;
  isBalanceLoading = this.transactionService.isBalanceLoading;

  logout() {
    this.authService.logout();
  }
  isDropdownOpen = signal(false);
  toggleDropdown() {
    this.isDropdownOpen.update((old) => !old);
  }
}
