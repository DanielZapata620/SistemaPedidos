import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ToastService {
  message = '';
  private timer?: number;

  show(message: string): void {
    this.message = message;
    window.clearTimeout(this.timer);
    this.timer = window.setTimeout(() => {
      this.message = '';
    }, 4500);
  }

  clear(): void {
    this.message = '';
    window.clearTimeout(this.timer);
  }
}
