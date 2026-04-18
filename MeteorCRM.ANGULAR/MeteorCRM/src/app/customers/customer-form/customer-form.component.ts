import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CustomerService } from '../../shared/services/customer.service';
import { CreateCustomerRequest } from '../../shared/models/customer.models';

@Component({
  selector: 'app-customer-form',
  templateUrl: './customer-form.component.html',
  styleUrls: ['./customer-form.component.css']
})
export class CustomerFormComponent {
  request: CreateCustomerRequest = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    company: '',
    address: '',
    notes: ''
  };
  isLoading = false;
  errorMessage = '';

  constructor(
    private customerService: CustomerService,
    private router: Router
  ) {}

  onSubmit(): void {
    this.isLoading = true;
    this.customerService.create(this.request).subscribe({
      next: () => this.router.navigate(['/customers']),
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to create customer.';
        this.isLoading = false;
      }
    });
  }
}