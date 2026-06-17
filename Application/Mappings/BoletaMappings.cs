using Application.DTOs.Boletas;
using Domain.Entities;

namespace Application.Mappings;

public static class BoletaMappings
{
    public static BoletaDto ToDto(this Boleta e)
        => new(e.Id, e.TalonarioId, e.Number, e.BuyerName, e.BuyerPhone, e.BuyerAddress, e.Sold);

    public static Boleta ToEntity(this CreateBoletaDto dto)
        => new()
        {
            TalonarioId = dto.TalonarioId,
            Number = dto.Number,
            BuyerName = dto.BuyerName,
            BuyerPhone = dto.BuyerPhone,
            BuyerAddress = dto.BuyerAddress,
            Sold = dto.Sold
        };

    public static void ApplyTo(this UpdateBoletaDto dto, Boleta e)
    {
        e.BuyerName = dto.BuyerName;
        e.BuyerPhone = dto.BuyerPhone;
        e.BuyerAddress = dto.BuyerAddress;
        e.Sold = dto.Sold;
    }
}
