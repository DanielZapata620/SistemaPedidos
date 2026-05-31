import { CommonModule } from '@angular/common';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Product } from '../../models/app.models';
import { ProductService } from '../../services/product.service';
import { ImageUrlService } from '../../services/image-url.service';

@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './admin-products.component.html'
})
export class AdminProductsComponent implements OnInit {
  @ViewChild('imageInput') imageInput?: ElementRef<HTMLInputElement>;

  products: Product[] = [];
  editing: Product | null = null;
  form = { name: '', description: '', price: 1, imageUrl: '/assets/img/art01.png' };
  selectedImage: File | null = null;
  selectedImageName = '';
  error = '';

  constructor(private readonly productService: ProductService, public readonly imageUrl: ImageUrlService) {}

  async ngOnInit(): Promise<void> {
    await this.load();
  }

  async load(): Promise<void> {
    this.products = (await this.productService.getAll()).filter(product => product.isActive);
  }

  edit(product: Product): void {
    this.editing = product;
    this.form = { name: product.name, description: product.description, price: product.price, imageUrl: product.imageUrl };
    this.selectedImage = null;
    this.selectedImageName = '';
  }

  async save(): Promise<void> {
    this.error = '';

    try {
      let saved: Product;
      if (this.editing) {
        saved = await this.productService.update({ ...this.editing, ...this.form, isActive: true });
      } else {
        saved = await this.productService.create(this.form);
      }

      if (this.selectedImage) {
        await this.productService.uploadImage(saved.id, this.selectedImage);
      }

      this.cancel();
      await this.load();
    } catch (error) {
      this.error = error instanceof Error ? error.message : 'No se pudo guardar';
    }
  }

  async delete(id: number): Promise<void> {
    await this.productService.delete(id);
    await this.load();
  }

  cancel(): void {
    this.editing = null;
    this.selectedImage = null;
    this.selectedImageName = '';
    if (this.imageInput) {
      this.imageInput.nativeElement.value = '';
    }
    this.form = { name: '', description: '', price: 1, imageUrl: '/assets/img/art01.png' };
  }

  selectImage(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0] ?? null;
    this.selectedImage = file;
    this.selectedImageName = file?.name ?? '';
  }
}
