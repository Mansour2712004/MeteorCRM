import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomersRoutingModule } from './customers-routing.module';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerFormComponent } from './customer-form/customer-form.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    CustomerListComponent,
    CustomerFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    CustomersRoutingModule,
    SharedModule
  ]
})
export class CustomersModule { }