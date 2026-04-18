import { Component, OnInit } from '@angular/core';
import { DealService } from '../../shared/services/deal.service';
import { Deal } from '../../shared/models/deal.models';

@Component({
  selector: 'app-deal-list',
  templateUrl: './deal-list.component.html',
  styleUrls: ['./deal-list.component.css']
})
export class DealListComponent implements OnInit {
  deals: Deal[] = [];
  isLoading = false;

  constructor(private dealService: DealService) {}

  ngOnInit(): void {
    this.loadDeals();
  }

  loadDeals(): void {
    this.isLoading = true;
    this.dealService.getAll().subscribe({
      next: (data) => {
        this.deals = data;
        this.isLoading = false;
      },
      error: () => this.isLoading = false
    });
  }

  deleteDeal(id: string): void {
    if (confirm('Are you sure you want to delete this deal?')) {
      this.dealService.delete(id).subscribe({
        next: () => this.loadDeals()
      });
    }
  }
}