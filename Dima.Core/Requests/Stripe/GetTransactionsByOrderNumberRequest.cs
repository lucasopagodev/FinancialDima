namespace Dima.Core.Requests.Stripe;

public class GetTransactionsByOrderNumberRequest : Request
{
    public string Number { get; set; } = String.Empty;
}