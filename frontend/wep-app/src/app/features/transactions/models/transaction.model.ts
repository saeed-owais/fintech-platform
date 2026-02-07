export interface Transaction {
    id: string,
    amount: number,
    transactionType: "Withdraw" | "Deposit" | "Transfer",
    status: "Completed" | "Failed",
    senderEmail: string,
    receiverEmail: string | null,
    createdAt: string
}

export interface HistoryRequest {
    transactionType: "Withdraw" | "Deposit" | "Transfer",
    fromDate: string,
    toDate: string,
    pageNumber: number,
    pageSize: number
}

export interface HistoryResponse {
    transactions: Transaction[],
    totalCount: number,
    pageNumber: number,
    pageSize: number,
    totalPages: number
}

export interface BalanceResponse {
    balance: number,
    walletId: string,
    isFrozen: boolean,
    lastUpdated: string
}

export interface WithdrawRequest {
    amount: number,
}

export interface WithdrawResponse {
    transactionId: string,
    amount: number,
    newBalance: number,
    createdAt: string
}


export interface DepositRequest {
    amount: number,
}

export interface DepositResponse {
    transactionId: string,
    amount: number,
    newBalance: number,
    createdAt: string
}

export interface TransferRequest {
    amount: number,
    receiverEmail: string
}

export interface TransferResponse {
    transactionId: string,
    senderEmail: string,
    receiverEmail: string,
    amount: number,
    newBalance: number,
    createdAt: string
}