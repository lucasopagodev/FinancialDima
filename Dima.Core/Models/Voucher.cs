namespace Dima.Core.Models;

public class Voucher
{
    public long Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public decimal Amount { get; set; }
}