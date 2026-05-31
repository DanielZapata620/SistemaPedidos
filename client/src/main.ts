import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, RouterOutlet, Routes } from '@angular/router';
import { Component } from '@angular/core';
import { LoginComponent } from './app/pages/login/login.component';
import { CommonModule } from '@angular/common';
import { ToastService } from './app/services/toast.service';
import { CustomerComponent } from './app/pages/customer/customer.component';
import { AdminDashboardComponent } from './app/pages/admin-dashboard/admin-dashboard.component';
import { AdminProductsComponent } from './app/pages/admin-products/admin-products.component';
import { AdminOrdersComponent } from './app/pages/admin-orders/admin-orders.component';
import { OrderDetailComponent } from './app/pages/order-detail/order-detail.component';
import { CustomerOrdersComponent } from './app/pages/customer-orders/customer-orders.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'cliente', component: CustomerComponent },
  { path: 'cliente/detalle', component: OrderDetailComponent },
  { path: 'cliente/pedidos', component: CustomerOrdersComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'admin/productos', component: AdminProductsComponent },
  { path: 'admin/pedidos', component: AdminOrdersComponent },
  { path: '**', redirectTo: 'login' }
];

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  template: `
    <div class="app-toast" *ngIf="toast.message">{{ toast.message }}</div>
    <router-outlet />
  `
})
class AppComponent {
  constructor(public readonly toast: ToastService) {}
}

bootstrapApplication(AppComponent, {
  providers: [provideRouter(routes)]
}).catch(error => console.error(error));
