import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Product } from '../models/app.models';

@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private readonly api: ApiService) {}

  getAll(): Promise<Product[]> {
    return this.api.get<Product[]>('/products');
  }

  create(product: Omit<Product, 'id' | 'isActive'>): Promise<Product> {
    return this.api.post<Product>('/products', product);
  }

  update(product: Product): Promise<Product> {
    return this.api.put<Product>(`/products/${product.id}`, product);
  }

  uploadImage(id: number, file: File): Promise<Product> {
    const formData = new FormData();
    formData.append('image', file);
    return this.api.upload<Product>(`/products/${id}/image`, formData);
  }

  delete(id: number): Promise<void> {
    return this.api.delete(`/products/${id}`);
  }
}
