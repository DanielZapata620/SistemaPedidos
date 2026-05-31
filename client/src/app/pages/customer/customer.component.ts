import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ProductService } from '../../services/product.service';
import { OrderService } from '../../services/order.service';
import { CartItem, Product, Order } from '../../models/app.models';
import { CartService } from '../../services/cart.service';
import { Router } from '@angular/router';
import { ImageUrlService } from '../../services/image-url.service';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './customer.component.html'
})
export class CustomerComponent implements OnInit {
  products: Product[] = [];
  cart: CartItem[] = [];
  orders: Order[] = [];
  lastOrder: Order | null = null;
  error = '';

  constructor(
    public readonly auth: AuthService,
    private readonly productsService: ProductService,
    private readonly orderService: OrderService,
    private readonly cartService: CartService,
    private readonly router: Router,
    public readonly imageUrl: ImageUrlService
  ) {}

  get total(): number {
    return this.cart.reduce((sum, item) => sum + item.product.price * item.quantity, 0);
  }

  async ngOnInit(): Promise<void> {
    this.products = (await this.productsService.getAll()).filter(product => product.isActive);
    this.cart = this.cartService.getItems();
    await this.loadOrders();
  }

  add(product: Product): void {
    const found = this.cart.find(item => item.product.id === product.id);
    if (found) {
      found.quantity++;
      this.cartService.save(this.cart);
      return;
    }

    this.cart.push({ product, quantity: 1 });
    this.cartService.save(this.cart);
  }

  getQuantity(productId: number): number {
    return this.cart.find(item => item.product.id === productId)?.quantity ?? 0;
  }

  remove(productId: number): void {
    this.cart = this.cart.filter(item => item.product.id !== productId);
    this.cartService.save(this.cart);
  }

  clear(): void {
    this.cart = [];
    this.cartService.clear();
    this.lastOrder = null;
  }

  async goToDetails(): Promise<void> {
    this.cartService.save(this.cart);
    await this.router.navigateByUrl('/cliente/detalle');
  }

  async goToOrders(): Promise<void> {
    await this.router.navigateByUrl('/cliente/pedidos');
  }

  statusText(status: Order['status']): string {
    const labels: Record<Order['status'], string> = {
      enviado: 'Enviado',
      'en preparacion': 'En preparacion',
      'listo para recoger': 'Listo para recoger'
    };

    return labels[status];
  }

  private async loadOrders(): Promise<void> {
    const user = this.auth.user;
    if (!user) return;
    this.orders = await this.orderService.getByUser(user.id);
  }
}
