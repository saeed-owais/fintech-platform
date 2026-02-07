import { Routes } from "@angular/router";

import { Dashboard } from "./components/dashboard/dashboard";
import { Deposit } from "./components/deposit/deposit";
import { Transfer } from "./components/transfer/transfer";
import { Withdraw } from "./components/withdraw/withdraw";
import { History } from "./components/history/history";

export const transactionRoutes : Routes = [
    {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
    {path: 'dashboard', component: Dashboard, title: 'Transactions Dashboard - FinTech' },
    {path: 'deposit', component: Deposit, title: 'Transactions Deposit - FinTech' },
    {path: 'withdraw', component: Withdraw, title: 'Transactions Withdraw - FinTech' },
    {path: 'transfer', component: Transfer, title: 'Transactions Transfer - FinTech' },
    {path: 'history', component: History, title: 'Transactions History - FinTech' }
];