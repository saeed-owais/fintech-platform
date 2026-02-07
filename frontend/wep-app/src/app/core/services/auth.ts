import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest, UserClaims } from '../models/auth.models';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = environment.apiUrl;
  private router = inject(Router)
  currentUser = signal<UserClaims | null>(null);
  isAuthenticated = computed(() => this.currentUser() !== null);

  private readonly http = inject(HttpClient);

  constructor() {
    const token = localStorage.getItem('token')
    if (token) {
      this.decodeAndNotify(token);
    }
  }

  private decodeAndNotify(token: string) {
    try {
      const decoded = jwtDecode<UserClaims>(token);

      const isExpired = decoded.exp * 1000 < Date.now();
      if (isExpired) {
        this.logout();
        return;
      }

      this.currentUser.set(decoded);
    }
    catch (error) {
      this.logout();
    }
  }

  login(data: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, data)
      .pipe(tap((response) => {
        localStorage.setItem('token', response.token);
        this.decodeAndNotify(response.token);
      }))
  }

  register(data: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, data)
      .pipe(tap((response) => {
        localStorage.setItem('token', response.token);
        this.decodeAndNotify(response.token);
      }))
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUser.set(null);
    this.router.navigate(['/auth/login']);
  }
}
