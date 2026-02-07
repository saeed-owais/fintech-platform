import { Component, computed, inject, signal } from '@angular/core';
import { AuthService } from '../../services/auth';
import { TransactionService } from '../../../features/transactions/services/transaction';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private authService = inject(AuthService);
  private transactionService = inject(TransactionService);

  balance = this.transactionService.balance
  logout() {
    this.authService.logout();
  }
  isDropdownOpen = signal(false);
  toggleDropdown() {
    this.isDropdownOpen.update((old) => !old);
  }
}
