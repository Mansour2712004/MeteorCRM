import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DealListComponent } from './deal-list/deal-list.component';
import { DealFormComponent } from './deal-form/deal-form.component';

const routes: Routes = [
  { path: '', component: DealListComponent },
  { path: 'new', component: DealFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DealsRoutingModule { }