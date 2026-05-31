import { CommonModule } from '@angular/common';
import { AfterViewChecked, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { UserRole } from '../../models/app.models';
import { GOOGLE_CLIENT_ID } from '../../config/google-auth.config';

declare global {
  interface Window {
    google?: {
      accounts: {
        id: {
          initialize: (options: { client_id: string; callback: (response: { credential: string }) => void }) => void;
          renderButton: (element: HTMLElement, options: { theme: string; size: string; width: number; text: string }) => void;
        };
      };
    };
  }
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit, AfterViewChecked {
  roleSelected = false;
  mode: 'login' | 'register' = 'login';
  role: UserRole = 'cliente';
  name = '';
  email = '';
  password = '';
  error = '';
  loading = false;
  private googleRenderedFor: UserRole | null = null;
  private googleRetry?: number;

  constructor(private readonly auth: AuthService, private readonly router: Router) {}

  ngOnInit(): void {
    const user = this.auth.user;
    if (user) {
      void this.router.navigateByUrl(user.role === 'admin' ? '/admin' : '/cliente');
    }
  }

  ngAfterViewChecked(): void {
    this.renderGoogleButton();
  }

  selectRole(role: UserRole): void {
    this.role = role;
    this.roleSelected = true;
    this.mode = 'login';
    this.email = '';
    this.password = '';
    this.name = '';
  }

  changeRole(): void {
    this.roleSelected = false;
    this.mode = 'login';
    this.error = '';
    this.googleRenderedFor = null;
  }

  async submit(): Promise<void> {
    this.error = '';
    this.loading = true;

    try {
      if (this.mode === 'login') {
        await this.auth.login(this.email, this.password, this.role);
      } else {
        const name = this.role === 'admin' ? 'Admin' : this.name;
        await this.auth.register(name, this.email, this.password, this.role, 'local');
      }
    } catch (error) {
      this.error = error instanceof Error ? error.message : 'Error de autenticacion';
    } finally {
      this.loading = false;
    }
  }

  private renderGoogleButton(): void {
    if (!this.roleSelected || this.googleRenderedFor === this.role) return;

    const container = document.getElementById('googleButton');
    if (!container) return;

    container.innerHTML = '';

    if (GOOGLE_CLIENT_ID.includes('TU_CLIENT_ID')) {
      this.error = 'Falta configurar el Client ID de Google.';
      return;
    }

    if (!window.google?.accounts?.id) {
      this.googleRetry = window.setTimeout(() => this.renderGoogleButton(), 400);
      return;
    }

    window.google.accounts.id.initialize({
      client_id: GOOGLE_CLIENT_ID,
      callback: response => void this.handleGoogleCredential(response.credential)
    });

    window.google.accounts.id.renderButton(container, {
      theme: 'outline',
      size: 'large',
      width: 360,
      text: 'continue_with'
    });

    this.googleRenderedFor = this.role;
  }

  private async handleGoogleCredential(credential: string): Promise<void> {
    this.error = '';
    this.loading = true;

    try {
      await this.auth.loginWithGoogle(credential, this.role);
    } catch (error) {
      this.error = error instanceof Error ? error.message : 'No se pudo iniciar con Google';
    } finally {
      this.loading = false;
    }
  }
}
