import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AccountComponent } from './features/account/account.component';
import { TransactionComponent } from './features/transaction/transaction.component';
import { UserComponent } from './features/user/user.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminAccountCreationComponent } from './admin-account-creation/admin-account-creation.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardComponent,canActivate:[AuthGuard] },
  { path: 'home', component: HomeComponent,canActivate:[AuthGuard] },
  { path: 'accounts', component: AccountComponent, canActivate: [AuthGuard] },
  { path: 'transactions', component: TransactionComponent, canActivate: [AuthGuard] },
  { path: 'users', component: UserComponent, canActivate: [AuthGuard] },
  { path: 'admin/create-account', component: AdminAccountCreationComponent, canActivate: [AuthGuard], data: { roles: ['Admin'] } },

  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
