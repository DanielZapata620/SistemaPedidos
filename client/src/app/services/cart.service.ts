import { Injectable } from '@angular/core';
import { CartItem } from '../models/app.models';

@Injectable({ providedIn: 'root' })
export class CartService {
  private readonly key = 'pedido_cart';

  getItems(): CartItem[] {
    const raw = localStorage.getItem(this.key);
    return raw ? JSON.parse(raw) as CartItem[] : [];
  }

  save(items: CartItem[]): void {
    localStorage.setItem(this.key, JSON.stringify(items));
  }

  clear(): void {
    localStorage.removeItem(this.key);
  }
}
