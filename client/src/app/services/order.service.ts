import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { CartItem, Order } from '../models/app.models';

@Injectable({ providedIn: 'root' })
export class OrderService {
  constructor(private readonly api: ApiService) {}

  getAll(): Promise<Order[]> {
    return this.api.get<Order[]>('/orders');
  }

  getByUser(userId: number): Promise<Order[]> {
    return this.api.get<Order[]>(`/orders/user/${userId}`);
  }

  create(userId: number, branchId: number, items: CartItem[]): Promise<Order> {
    return this.api.post<Order>('/orders', {
      userId,
      branchId,
      items: items.map(item => ({ productId: item.product.id, quantity: item.quantity }))
    });
  }

  updateStatus(id: number, status: Order['status']): Promise<Order> {
    return this.api.put<Order>(`/orders/${id}/status`, { status });
  }
}
