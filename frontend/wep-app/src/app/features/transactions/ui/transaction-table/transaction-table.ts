import { Component, input } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { Transaction } from '../../models/transaction.model';

@Component({
    selector: 'app-transaction-table',
    imports: [DatePipe, DecimalPipe],
    templateUrl: './transaction-table.html',
})
export class TransactionTable {
    transactions = input.required<Transaction[]>();
    showHeader = input(true);
}
