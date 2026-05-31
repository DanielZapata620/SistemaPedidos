import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { DashboardService } from '../../services/dashboard.service';
import { Branch, Dashboard, StoreInfo } from '../../models/app.models';
import { BranchService } from '../../services/branch.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './admin-dashboard.component.html'
})
export class AdminDashboardComponent implements OnInit {
  dashboard: Dashboard | null = null;
  storeInfo: StoreInfo | null = null;
  branches: Branch[] = [];
  showBranchForm = false;
  branchForm = { id: 0, name: '', address: '', latitude: 19.4326, longitude: -99.1332, username: '', password: '' };

  constructor(
    public readonly auth: AuthService,
    private readonly dashboardService: DashboardService,
    private readonly branchService: BranchService
  ) {}

  async ngOnInit(): Promise<void> {
    this.dashboard = await this.dashboardService.getDashboard();
    this.storeInfo = await this.dashboardService.getStoreInfo();
    this.branches = await this.branchService.getAll();
  }

  editBranch(branch: Branch): void {
    this.showBranchForm = true;
    this.branchForm = { ...branch, password: '' };
  }

  newBranch(): void {
    this.showBranchForm = true;
    this.branchForm = { id: 0, name: '', address: '', latitude: 19.4326, longitude: -99.1332, username: '', password: '' };
  }

  async saveBranch(): Promise<void> {
    if (this.branchForm.id) {
      await this.branchService.update(this.branchForm);
    } else {
      await this.branchService.create(this.branchForm);
    }
    this.branches = await this.branchService.getAll();
    this.showBranchForm = false;
  }
}
