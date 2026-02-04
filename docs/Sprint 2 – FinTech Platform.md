# Sprint 2 Plan – FinTech Platform (Version 1)

---

## 🟦 Sprint Overview

| Field | Value |
|-------|-------|
| **Sprint Name** | Sprint 2 – Core Transactions |
| **Sprint Duration** | 10–14 Days |

---

## 🎯 Sprint Goal

Enable users to perform core wallet transactions: deposits, withdrawals, and transfers to other users.

At the end of this sprint, the system must allow a user to:
- Deposit funds into their wallet
- Withdraw funds from their wallet
- Transfer funds to another user's wallet
- View their updated wallet balance (calculated from transactions)
- View their transaction history

---

## 📦 User Stories Included

### US-4: Deposit Funds
**As a user**, I want to deposit funds into my wallet so that I can have money available to use or transfer.

#### Acceptance Criteria
- User can deposit a valid positive amount
- Transaction is created with type "Deposit" and status "Completed"
- Wallet balance reflects the deposit
- Deposits to frozen wallets are rejected

---

### US-5: Withdraw Funds
**As a user**, I want to withdraw funds from my wallet so that I can access my money.

#### Acceptance Criteria
- User can withdraw up to their current balance
- System prevents overdraw (insufficient balance)
- Transaction is created with type "Withdrawal" and status "Completed"
- Wallet balance is updated accordingly
- Withdrawals from frozen wallets are rejected

---

### US-6: Transfer Funds
**As a user**, I want to transfer funds to another user's wallet so that I can send money to others.

#### Acceptance Criteria
- User can transfer funds to another user by wallet ID or email
- System validates sender has sufficient balance
- System validates recipient wallet exists and is not frozen
- Two transactions are created atomically (debit sender, credit receiver)
- Both wallet balances are updated correctly
- Failed transfers do not affect any balances (rollback)

---

### US-7: View Wallet Balance
**As a user**, I want to view my current wallet balance so that I know how much money I have.

#### Acceptance Criteria
- Balance is calculated as: Sum of incoming transactions - Sum of outgoing transactions
- Balance is displayed on the wallet/dashboard page
- Balance updates after each transaction

---

### US-8: View Transaction History
**As a user**, I want to view my transaction history so that I can track my financial activities.

#### Acceptance Criteria
- User can see all their transactions
- Transactions show: type, amount, date, status, and related party (for transfers)
- History supports pagination
- History supports filtering by transaction type and date range

---

## ✅ Definition of Done (Applied)

All User Stories in this sprint must comply with the global Definition of Done, including:
- Backend + Frontend completion
- Acceptance Criteria satisfied
- End-to-end functionality verified

---

## 🛠️ Sprint Tasks Breakdown

### 🔹 Backend Tasks (.NET 8)

#### Domain Layer
- [ ] Create `Transaction` entity with properties:
  - Id, WalletId, Type (Deposit/Withdrawal/TransferIn/TransferOut), Amount, Status (Pending/Completed/Failed), RelatedTransactionId (for transfers), Description, CreatedAt
- [ ] Create `TransactionType` enum (Deposit, Withdrawal, TransferIn, TransferOut)
- [ ] Create `TransactionStatus` enum (Pending, Completed, Failed)
- [ ] Add navigation property from Wallet to Transactions

#### Application Layer
- [ ] Create `ITransactionRepository` interface
- [ ] Implement Deposit command (CQRS pattern)
  - `DepositCommand` + `DepositCommandHandler`
- [ ] Implement Withdraw command
  - `WithdrawCommand` + `WithdrawCommandHandler`
- [ ] Implement Transfer command
  - `TransferCommand` + `TransferCommandHandler`
- [ ] Implement Get Balance query
  - `GetBalanceQuery` + `GetBalanceQueryHandler`
- [ ] Implement Get Transaction History query
  - `GetTransactionHistoryQuery` + `GetTransactionHistoryQueryHandler`
- [ ] Add validation for all transaction commands

#### Infrastructure Layer
- [ ] Implement `TransactionRepository`
- [ ] Create `TransactionConfiguration` for EF Core
- [ ] Create EF Core migration for Transaction table
- [ ] Implement balance calculation logic (aggregate transactions)

#### API Layer
- [ ] Create `TransactionController` with endpoints:
  - `POST /api/transactions/deposit`
  - `POST /api/transactions/withdraw`
  - `POST /api/transactions/transfer`
  - `GET /api/wallet/balance`
  - `GET /api/transactions/history`
- [ ] Add appropriate authorization attributes

---

### 🔹 Database Tasks

#### New Tables
- [ ] Design `Transactions` table with columns:
  - Id (GUID, PK)
  - WalletId (GUID, FK)
  - Type (int/enum)
  - Amount (decimal)
  - Status (int/enum)
  - RelatedTransactionId (GUID, nullable - for linking transfer pairs)
  - Description (nvarchar, nullable)
  - CreatedAt (datetime)

#### Data Integrity
- [ ] Add foreign key constraint: Transaction → Wallet
- [ ] Add check constraint: Amount > 0
- [ ] Add index on WalletId for performance
- [ ] Add index on CreatedAt for sorting

---

### 🔹 Frontend Tasks (Angular)

#### Core Components
- [ ] Create **Dashboard** page/component
  - Display wallet balance prominently
  - Quick action buttons for deposit/withdraw/transfer
  - Recent transactions summary

#### Transaction Components
- [ ] Create **Deposit** page/component
  - Amount input with validation
  - Submit button
  - Success/Error feedback

- [ ] Create **Withdraw** page/component
  - Amount input with validation
  - Display current balance
  - Submit button
  - Insufficient balance warning

- [ ] Create **Transfer** page/component
  - Recipient identifier input (email or wallet ID)
  - Amount input with validation
  - Display current balance
  - Submit button
  - Confirmation dialog before transfer

- [ ] Create **Transaction History** page/component
  - Transaction list with pagination
  - Filter by type dropdown
  - Date range filter
  - Transaction details (type, amount, date, status)

#### Services
- [ ] Create `TransactionService` with methods:
  - `deposit(amount: number)`
  - `withdraw(amount: number)`
  - `transfer(recipientId: string, amount: number)`
  - `getBalance()`
  - `getTransactionHistory(filters, pagination)`

- [ ] Create `WalletService` with methods:
  - `getWalletInfo()`

#### Models
- [ ] Create `Transaction` interface
- [ ] Create `TransactionType` enum
- [ ] Create `TransactionStatus` enum
- [ ] Create `TransactionHistoryResponse` interface (paginated)

#### Routing & Navigation
- [ ] Add routes for new pages
- [ ] Update navigation/sidebar with new menu items
- [ ] Protect routes with AuthGuard

---

## 🔄 Workflow

1. Design and create Transaction entity
2. Create database migration and apply
3. Implement backend APIs (deposit → withdraw → transfer → balance → history)
4. Create frontend services
5. Build frontend components
6. Connect frontend to backend
7. End-to-end testing

---

## 🧪 Verification & Testing

### Automated Testing
- [ ] Unit tests for transaction commands
- [ ] Unit tests for balance calculation
- [ ] Integration tests for transaction endpoints
- [ ] Test atomic transfer operations

### Manual End-to-End Testing
1. **Deposit Test**
   - Login as user
   - Navigate to deposit page
   - Deposit 100.00
   - Verify balance shows 100.00
   - Verify transaction appears in history

2. **Withdraw Test**
   - With balance of 100.00, withdraw 30.00
   - Verify balance shows 70.00
   - Attempt to withdraw 100.00 (should fail with insufficient balance)

3. **Transfer Test**
   - Create second test user
   - Transfer 20.00 from first user to second
   - Verify first user balance is 50.00
   - Verify second user balance is 20.00
   - Verify both users see the transfer in history

4. **Frozen Wallet Test**
   - Admin freezes a user's wallet
   - Attempt deposit/withdraw/transfer (all should fail)

5. **Transaction History Test**
   - Verify all transactions are listed
   - Test pagination with many transactions
   - Test filters (by type, by date)

---

## 📌 Sprint Deliverables

- ✅ Working deposit functionality
- ✅ Working withdrawal functionality
- ✅ Working transfer functionality
- ✅ Balance calculation from transactions
- ✅ Transaction history with pagination
- ✅ Backend and frontend fully integrated

---

## 🏗️ Technical Considerations

### Transaction Atomicity
- Use database transactions (`BeginTransaction`, `Commit`, `Rollback`)
- For transfers: both debit and credit must succeed or both fail
- Use `TransactionScope` or EF Core's `SaveChanges` within a transaction

### Balance Calculation
- **NEVER store balance directly** - always calculate from transactions
- Use efficient aggregate query:
  ```sql
  SELECT 
    SUM(CASE WHEN Type IN ('Deposit', 'TransferIn') THEN Amount ELSE 0 END) -
    SUM(CASE WHEN Type IN ('Withdrawal', 'TransferOut') THEN Amount ELSE 0 END)
  FROM Transactions
  WHERE WalletId = @walletId AND Status = 'Completed'
  ```

### Concurrency Handling
- Use optimistic concurrency for balance checks
- Implement idempotency keys for duplicate request prevention (future enhancement)

---

## 🧠 Notes

- This sprint focuses on core transaction mechanics
- Audit logging will be addressed in Sprint 3
- Admin transaction monitoring will be addressed in Sprint 3
- No real payment gateway - deposits are simulated

---

## ✔ Sprint Completion Criteria

Sprint 2 is considered complete when:
- All included User Stories meet their Acceptance Criteria
- All User Stories meet the Definition of Done
- The system produces a working, demonstrable increment
- Users can successfully deposit, withdraw, and transfer funds
- Transaction history is fully functional

---

## 📅 Sprint Backlog Summary

| Story | Story Points | Priority |
|-------|--------------|----------|
| US-4: Deposit Funds | 3 | High |
| US-5: Withdraw Funds | 3 | High |
| US-6: Transfer Funds | 5 | High |
| US-7: View Wallet Balance | 2 | High |
| US-8: View Transaction History | 3 | Medium |
| **Total** | **16** | - |
