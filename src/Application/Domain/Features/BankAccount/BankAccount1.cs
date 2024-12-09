namespace Application.Domain.Features.BankAccount;

using Application.Domain.Features.Transaction;

public sealed class BankAccount1
{
    public Transaction1 Transaction { get; set; }

    public BankAccount1()
    {
        Transaction = new Transaction1();
    }
}
