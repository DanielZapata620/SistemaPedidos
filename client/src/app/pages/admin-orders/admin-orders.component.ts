import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Order } from '../../models/app.models';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './admin-orders.component.html'
})
export class AdminOrdersComponent implements OnInit {
  orders: Order[] = [];

  constructor(private readonly orderService: OrderService, private readonly auth: AuthService) {}

  async ngOnInit(): Promise<void> {
    await this.load();
  }

  async load(): Promise<void> {
    const orders = await this.orderService.getAll();
    const user = this.auth.user;
    this.orders = user?.role === 'sucursal' && user.branchId
      ? orders.filter(order => order.branchId === user.branchId)
      : orders;
  }

  async setStatus(order: Order, status: Order['status']): Promise<void> {
    await this.orderService.updateStatus(order.id, status);
    await this.load();
  }

  statusText(status: Order['status']): string {
    const labels: Record<Order['status'], string> = {
      enviado: 'Enviado',
      'en preparacion': 'En preparacion',
      'listo para recoger': 'Listo para recoger'
    };

    return labels[status];
  }
}
