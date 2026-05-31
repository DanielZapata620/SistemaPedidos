export type UserRole = 'cliente' | 'admin' | 'sucursal';

export interface User {
  id: number;
  name: string;
  email: string;
  role: UserRole;
  authProvider: 'local' | 'google';
  branchId?: number;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  isActive: boolean;
}

export interface CartItem {
  product: Product;
  quantity: number;
}

export interface Branch {
  id: number;
  name: string;
  address: string;
  latitude: number;
  longitude: number;
  username: string;
}

export interface OrderItem {
  productId: number;
  productName: string;
  unitPrice: number;
  quantity: number;
  subtotal: number;
}

export interface Order {
  id: number;
  userId: number;
  branchId: number;
  branchName: string;
  branchAddress: string;
  customerName: string;
  customerEmail: string;
  status: 'enviado' | 'en preparacion' | 'listo para recoger';
  deliveryType: string;
  paymentMethod: string;
  total: number;
  createdAt: string;
  items: OrderItem[];
}

export interface Dashboard {
  totalProducts: number;
  totalOrders: number;
  pendingOrders: number;
  totalSales: number;
}

export interface StoreInfo {
  storeName: string;
  address: string;
  latitude: number;
  longitude: number;
  weatherSummary: string;
  pickupPaymentFee: number;
}
