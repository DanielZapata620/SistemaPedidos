import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ImageUrlService {
  private readonly apiOrigin = 'http://localhost:5032';

  resolve(url: string): string {
    if (!url) return '/assets/img/art01.png';
    if (url.startsWith('/uploads/')) return `${this.apiOrigin}${url}`;
    return url;
  }
}
