import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TaskService } from '../../shared/services/task.service';
import { CustomerService } from '../../shared/services/customer.service';
import { CreateTaskRequest } from '../../shared/models/task.models';
import { Customer } from '../../shared/models/customer.models';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  request: CreateTaskRequest = {
    title: '',
    description: '',
    priority: 2,
    customerId: undefined
  };
  customers: Customer[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(
    private taskService: TaskService,
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
    this.taskService.create(this.request).subscribe({
      next: () => this.router.navigate(['/tasks']),
      error: (err) => {
        this.errorMessage = err.error?.message || 'Failed to create task.';
        this.isLoading = false;
      }
    });
  }
}