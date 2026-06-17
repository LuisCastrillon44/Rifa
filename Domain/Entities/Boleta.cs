using Domain.Common;

namespace Domain.Entities;

public class Boleta : BaseEntity
{
    public long TalonarioId { get; set; }
    public int Number { get; set; }
    public string? BuyerName { get; set; }
    public string? BuyerPhone { get; set; }
    public string? BuyerAddress { get; set; }
    public bool Sold { get; set; }

    public Talonario Talonario { get; set; } = null!;
}
