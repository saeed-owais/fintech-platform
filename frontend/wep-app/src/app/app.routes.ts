import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { AuthLayout } from './core/layouts/auth-layout/auth-layout';
import { transactionRoutes } from './features/transactions/transactions.routes';
import { MainLayout } from './core/layouts/main-layout/main-layout';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
  {
    path: 'auth',
    component: AuthLayout,
    children: [
      { path: 'login', component: Login, title: 'Login - FinTech' },
      { path: 'register', component: Register, title: 'Register - FinTech' },
      { path: '', redirectTo: 'login', pathMatch: 'full' },
    ],
  },
  { path: '', component: MainLayout, canActivate: [authGuard], children: transactionRoutes },

  { path: '**', redirectTo: '' },
];
