import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Dashboard, StoreInfo } from '../models/app.models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  constructor(private readonly api: ApiService) {}

  getDashboard(): Promise<Dashboard> {
    return this.api.get<Dashboard>('/dashboard');
  }

  getStoreInfo(): Promise<StoreInfo> {
    return this.api.get<StoreInfo>('/external/store-info');
  }
}
