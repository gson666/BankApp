import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountComponent } from './account/account.component';
import { TransactionComponent } from './transaction/transaction.component';
import { UserComponent } from './user/user.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SharedModule } from '../shared/shared.module';
import { MaterialModule } from '../material/material.module';
import { RouterModule } from '@angular/router';
import { CoreModule } from '../core/core.module';
import { HomeComponent } from '../home/home.component';

@NgModule({
  declarations: [
    AccountComponent,
    TransactionComponent,
    UserComponent,
    DashboardComponent,
    HomeComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MaterialModule,
    RouterModule,
    CoreModule
  ],
  exports: [
    AccountComponent,
    TransactionComponent,
    UserComponent,
    DashboardComponent,
    HomeComponent
  ]
})
export class FeaturesModule { }
