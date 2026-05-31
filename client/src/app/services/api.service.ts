import { Injectable } from '@angular/core';
import { ToastService } from './toast.service';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly baseUrl = 'http://localhost:5032/api';

  constructor(private readonly toast: ToastService) {}

  get<T>(path: string): Promise<T> {
    return this.request<T>(path, { method: 'GET' });
  }

  post<T>(path: string, body: unknown): Promise<T> {
    return this.request<T>(path, { method: 'POST', body: JSON.stringify(body) });
  }

  put<T>(path: string, body: unknown): Promise<T> {
    return this.request<T>(path, { method: 'PUT', body: JSON.stringify(body) });
  }

  delete(path: string): Promise<void> {
    return this.request<void>(path, { method: 'DELETE' });
  }

  upload<T>(path: string, formData: FormData): Promise<T> {
    return this.request<T>(path, { method: 'POST', body: formData }, false);
  }

  private async request<T>(path: string, options: RequestInit, isJson = true): Promise<T> {
    const token = localStorage.getItem('token');
    const response = await fetch(`${this.baseUrl}${path}`, {
      ...options,
      headers: {
        ...(isJson ? { 'Content-Type': 'application/json' } : {}),
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
        ...options.headers
      }
    });

    if (!response.ok) {
      const message = await this.getErrorMessage(response);
      this.toast.show(message);
      throw new Error(message);
    }

    if (response.status === 204) {
      return undefined as T;
    }

    return response.json() as Promise<T>;
  }

  private async getErrorMessage(response: Response): Promise<string> {
    const text = await response.text();
    if (!text) return 'No se pudo completar la solicitud';

    try {
      const json = JSON.parse(text) as { message?: string; title?: string; errors?: Record<string, string[]> };
      if (json.message) return json.message;
      if (json.errors) return Object.values(json.errors).flat().join(' ');
      if (json.title) return json.title;
    } catch {
      return text.length > 180 ? 'No se pudo completar la solicitud.' : text;
    }

    return 'No se pudo completar la solicitud';
  }
}
