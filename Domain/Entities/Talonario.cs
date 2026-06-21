using Domain.Common;

namespace Domain.Entities;

public class Talonario : BaseEntity
{
    public long UserId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int BoletasNumber { get; set; }
    public decimal BoletasValue { get; set; }
    public DateTime LotteryDate { get; set; }
    public string? LotteryNumber { get; set; }
    public string Jackpot { get; set; } = null!;

    public Usuario Usuario { get; set; } = null!;
    public ICollection<Boleta> Boletas { get; set; } = new List<Boleta>();
}
