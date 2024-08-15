export interface Transaction {
    id: string;
    name: string;
    amount: number;
    date: string;
    paymentChannel: string;
    category: string;
    type: string;
    senderBankId: string;
    receiverBankId: string;
  }
  