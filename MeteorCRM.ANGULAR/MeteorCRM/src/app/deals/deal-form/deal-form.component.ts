import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DealService } from '../../shared/services/deal.service';
import { CustomerService } from '../../shared/services/customer.service';
import { CreateDealRequest } from '../../shared/models/deal.models';
import { Customer } from '../../shared/models/customer.models';

@Component({
  selector: 'app-deal-form',
  templateUrl: './deal-form.component.html',
  styleUrls: ['./deal-form.component.css']
})
export class DealFormComponent implements OnInit {
  request: CreateDealRequest = {
    title: '',
    value: 0,
    customerId: '',
    notes: ''
  };
  customers: Customer[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private dealService: DealService,
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.customerService.getAll().subscribe({
      next: (data) => this.customers = data
    });
  }

  onSubmit(): void {
    this.isLoading = true;
    this.dealService.create(this.request).subscribe({
      next: () => this.router.navigate(['/deals']),
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to create deal.';
        this.isLoading = false;
      }
    });
  }
}