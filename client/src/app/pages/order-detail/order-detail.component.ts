import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { BranchService } from '../../services/branch.service';
import { CartService } from '../../services/cart.service';
import { OrderService } from '../../services/order.service';
import { Branch, CartItem } from '../../models/app.models';
import { ImageUrlService } from '../../services/image-url.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-detail.component.html'
})
export class OrderDetailComponent implements OnInit {
  items: CartItem[] = [];
  branches: Branch[] = [];
  selectedBranchId = 0;
  error = '';

  constructor(
    private readonly cartService: CartService,
    private readonly branchService: BranchService,
    private readonly orderService: OrderService,
    private readonly auth: AuthService,
    private readonly router: Router,
    public readonly imageUrl: ImageUrlService
  ) {}

  get total(): number {
    return this.items.reduce((sum, item) => sum + item.product.price * item.quantity, 0);
  }

  async ngOnInit(): Promise<void> {
    this.items = this.cartService.getItems();
    this.branches = await this.branchService.getAll();
    this.selectedBranchId = this.branches[0]?.id ?? 0;
  }

  async back(): Promise<void> {
    await this.router.navigateByUrl('/cliente');
  }

  async cancel(): Promise<void> {
    this.cartService.clear();
    await this.router.navigateByUrl('/cliente');
  }

  async confirm(): Promise<void> {
    const user = this.auth.user;
    if (!user || this.items.length === 0 || this.selectedBranchId === 0) return;

    try {
      await this.orderService.create(user.id, this.selectedBranchId, this.items);
      this.cartService.clear();
      await this.router.navigateByUrl('/cliente/pedidos');
    } catch (error) {
      this.error = error instanceof Error ? error.message : 'No se pudo confirmar el pedido';
    }
  }
}
