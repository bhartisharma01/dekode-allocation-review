import { Component, inject, signal, Signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  loading = signal(false);
  errorMessage = signal('');

  loginForm: FormGroup;
  private fb = inject(FormBuilder);
  private authService= inject(AuthService);
  private router = inject(Router);
  
  constructor() {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid || this.loading()) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set('');

    this.authService.login({
      username: this.loginForm.value.username!,
      password: this.loginForm.value.password!
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/runs']);
      },
      error: (error) => {
        this.loading.set(false);
        this.errorMessage.set(
          error.error?.message || 'Unable to sign in. Please try again.'
        );
      }
    });
  }
}