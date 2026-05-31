import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Branch } from '../models/app.models';

@Injectable({ providedIn: 'root' })
export class BranchService {
  constructor(private readonly api: ApiService) {}

  getAll(): Promise<Branch[]> {
    return this.api.get<Branch[]>('/branches');
  }

  create(branch: Omit<Branch, 'id'> & { password: string }): Promise<Branch> {
    return this.api.post<Branch>('/branches', branch);
  }

  update(branch: Branch & { password?: string }): Promise<Branch> {
    return this.api.put<Branch>(`/branches/${branch.id}`, branch);
  }

  delete(id: number): Promise<void> {
    return this.api.delete(`/branches/${id}`);
  }
}
