import { Component, OnInit } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Account } from '../shared/models/account';
import { AuthService } from '../core/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  account: Account | null = null;
  greeting: string = '';

  constructor(private accountService: AccountService,private auth:AuthService) {}

  ngOnInit(): void {
    this.loadAccount();
    this.setGreeting();
  }

  loadAccount() {
    const userId = this.auth.getCurrentUser();
    console.log(userId);
    if(userId){
      this.accountService.getAccountById(userId).subscribe(account => {
        this.account = account;
      });
    }
    
  }

  setGreeting() {
    const currentHour = new Date().getHours();
    if (currentHour < 12) {
      this.greeting = 'Good Morning';
    } else if (currentHour < 18) {
      this.greeting = 'Good Afternoon';
    } else {
      this.greeting = 'Good Evening';
    }
  }
}
