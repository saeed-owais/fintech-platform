import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private authService = inject(AuthService);

  logout() {
    this.authService.logout();
  }
  isDropdownOpen = signal(false);
  toggleDropdown() {
    this.isDropdownOpen.update((old) => !old);
  }
}
