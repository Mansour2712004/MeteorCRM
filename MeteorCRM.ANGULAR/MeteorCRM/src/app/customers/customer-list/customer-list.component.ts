import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../../shared/services/customer.service';
import { Customer } from '../../shared/models/customer.models';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrls: ['./customer-list.component.css']
})
export class CustomerListComponent implements OnInit {
  customers: Customer[] = [];
  isLoading = false;

  constructor(private customerService: CustomerService) {}

  ngOnInit(): void {
    this.loadCustomers();
  }

  loadCustomers(): void {
    this.isLoading = true;
    this.customerService.getAll().subscribe({
      next: (data) => {
        this.customers = data;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  deleteCustomer(id: string): void {
    if (confirm('Are you sure you want to delete this customer?')) {
      this.customerService.delete(id).subscribe({
        next: () => this.loadCustomers()
      });
    }
  }
}