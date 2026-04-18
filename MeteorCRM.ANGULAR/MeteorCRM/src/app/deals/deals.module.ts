import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DealsRoutingModule } from './deals-routing.module';
import { DealListComponent } from './deal-list/deal-list.component';
import { DealFormComponent } from './deal-form/deal-form.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    DealListComponent,
    DealFormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DealsRoutingModule,
    SharedModule
  ]
})
export class DealsModule { }