import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { OrderService } from '../../services/order.service';
import { Branch, Order } from '../../models/app.models';
import { BranchService } from '../../services/branch.service';

@Component({
  selector: 'app-customer-orders',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './customer-orders.component.html'
})
export class CustomerOrdersComponent implements OnInit {
  orders: Order[] = [];
  branches: Branch[] = [];

  constructor(private readonly auth: AuthService, private readonly orderService: OrderService, private readonly branchService: BranchService) {}

  async ngOnInit(): Promise<void> {
    const user = this.auth.user;
    if (!user) return;
    const [orders, branches] = await Promise.all([
      this.orderService.getByUser(user.id),
      this.branchService.getAll()
    ]);
    this.orders = orders;
    this.branches = branches;
  }

  statusText(status: Order['status']): string {
    const labels: Record<Order['status'], string> = {
      enviado: 'Enviado',
      'en preparacion': 'En preparacion',
      'listo para recoger': 'Listo para recoger'
    };
    return labels[status];
  }

  mapsUrl(order: Order): string {
    const branch = this.branches.find(item => item.id === order.branchId);
    const query = branch ? `${branch.latitude},${branch.longitude}` : order.branchAddress;
    return `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(query)}`;
  }
}
