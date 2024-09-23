using Dima.Core.Enums;

namespace Dima.Core.Models;

public class Order
{
  public long Id { get; set; }
  public string Number { get; set; } = Guid.NewGuid().ToString("N")[..8];
  
  public long ProductId { get; set; }
  public Product Product { get; set; } = null!;

  public long? VoucherId { get; set; }
  public Voucher? Voucher { get; set; }

  public string UserId { get; set; } = string.Empty;

  public decimal Total => Product.Price - (Voucher?.Amount ?? 0);
}
