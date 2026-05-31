import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from './api.service';
import { AuthResponse, User, UserRole } from '../models/app.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private readonly api: ApiService, private readonly router: Router) {}

  get user(): User | null {
    const raw = localStorage.getItem('user');
    return raw ? JSON.parse(raw) as User : null;
  }

  async login(email: string, password: string, role: UserRole): Promise<void> {
    const response = await this.api.post<AuthResponse>('/auth/login', { email, password, role });
    this.saveSession(response);
    await this.router.navigateByUrl(response.user.role === 'cliente' ? '/cliente' : '/admin');
  }

  async register(name: string, email: string, password: string, role: UserRole, authProvider: 'local' | 'google'): Promise<void> {
    const response = await this.api.post<AuthResponse>('/auth/register', { name, email, password, role, authProvider });
    this.saveSession(response);
    await this.router.navigateByUrl(response.user.role === 'cliente' ? '/cliente' : '/admin');
  }

  async loginWithGoogle(credential: string, role: UserRole): Promise<void> {
    const response = await this.api.post<AuthResponse>('/auth/google', { credential, role });
    this.saveSession(response);
    await this.router.navigateByUrl(response.user.role === 'cliente' ? '/cliente' : '/admin');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    void this.router.navigateByUrl('/login');
  }

  private saveSession(response: AuthResponse): void {
    localStorage.removeItem('pedido_cart');
    localStorage.setItem('token', response.token);
    localStorage.setItem('user', JSON.stringify(response.user));
  }
}
