import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Register } from './features/auth/register/register';
import { AuthLayout } from './shared/layouts/auth-layout/auth-layout';

export const routes: Routes = [
   {
    path: 'auth', 
    component: AuthLayout, 
    children: [
      { path: 'login', component: Login, title: 'Login - FinTech' },
      { path: 'register', component: Register, title: 'Register - FinTech' },
      { path: '', redirectTo: 'login', pathMatch: 'full' },
    ]
  },
  { path: '', redirectTo: 'auth/login', pathMatch: 'full' }  
];
