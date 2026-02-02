import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth';
import { RegisterRequest } from '../../../core/models/auth.models';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly fb = inject(FormBuilder);
  private readonly router = inject(Router);

  isLoading = signal(false);
  registerForm = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  get nameInvalid() {
    return this.registerForm.get('name')?.invalid && this.registerForm.get('name')?.touched;
  }

  get emailInvalid() {
    return this.registerForm.get('email')?.invalid && this.registerForm.get('email')?.touched;
  }

  get passwordInvalid() {
    return this.registerForm.get('password')?.invalid && this.registerForm.get('password')?.touched;
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.isLoading.set(true);
    this.authService.register(this.registerForm.value as RegisterRequest).subscribe({
      next: () => {
        this.isLoading.set(false);
        // this.router.navigate(['/app/dashboard']);
      },
      error: (error) => {
        this.isLoading.set(false);
        console.error('Registration failed:', error);
        // TODO: Show error message to user
      }
    });
  }
}
