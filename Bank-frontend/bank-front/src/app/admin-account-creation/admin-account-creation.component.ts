// src/app/admin-account-creation/admin-account-creation.component.ts
import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Account } from '../shared/models/account';

@Component({
  selector: 'app-admin-account-creation',
  templateUrl: './admin-account-creation.component.html',
  styleUrls: ['./admin-account-creation.component.css']
})
export class AdminAccountCreationComponent {
  account: Account = {
    id: '',
    name: '',
    currentBalance: 0,
    availableBalance: 0,
    userId: '',
    mask: '',
    institutionId: '',
    type: '',
    subtype: '',
    shareableId: ''
  };

  constructor(private accountService: AccountService) {}

  createAccount() {
    this.accountService.createAccount(this.account).subscribe(
      () => {
        alert('Account created successfully');
        // Reset form or navigate away
      },
      error => {
        console.error('Error creating account:', error);
        alert('Failed to create account');
      }
    );
  }
}
