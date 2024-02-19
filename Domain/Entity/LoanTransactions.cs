﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class LoanTransactions
    {
        public Guid Code { get; private set; }
        public Guid CodeLoan { get; private set; }
        public string NameBankSafe { get; private set; }
        public int NumberOfInstallments { get; private set; }
        public decimal Amount { get; private set; }
        public Loan Loan { get; private set; }
        public BankSafe BankSafe { get; private set; }
        public LoanTransactions(Guid codeLoan , string nameBankSafe , int numberOfInstallments
            , decimal amount)
        {
            Code = Guid.NewGuid();
            CodeLoan= codeLoan;
            NameBankSafe= nameBankSafe;
            NumberOfInstallments= numberOfInstallments;
            Amount= amount;
        }
        public void Update(int numberOfInstallments, decimal amount)
        {
            NumberOfInstallments = numberOfInstallments;
            Amount = amount;
        }
    }
}
