import { Component, OnInit } from '@angular/core';
import { Transaction } from 'src/app/shared/models/transaction';
import { TransactionService } from 'src/app/services/transaction.service';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent implements OnInit {

  transactions:Transaction[] = [];
  constructor(private transactionService:TransactionService){}


  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions() {
    this.transactionService.getTransactions().subscribe(data=>{
      this.transactions = data;
    });
  }



}
