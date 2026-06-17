using Application.DTOs.Talonarios;
using Domain.Entities;

namespace Application.Mappings;

public static class TalonarioMappings
{
    public static TalonarioDto ToDto(this Talonario e)
        => new(e.Id, e.UserId, e.Title, e.Description, e.BoletasNumber, e.BoletasValue,
            e.LotteryDate, e.LotteryNumber, e.Jackpot);

    public static Talonario ToEntity(this CreateTalonarioDto dto)
        => new()
        {
            UserId = dto.UserId,
            Title = dto.Title,
            Description = dto.Description,
            BoletasNumber = dto.BoletasNumber,
            BoletasValue = dto.BoletasValue,
            LotteryDate = dto.LotteryDate,
            LotteryNumber = dto.LotteryNumber,
            Jackpot = dto.Jackpot
        };

    public static void ApplyTo(this UpdateTalonarioDto dto, Talonario e)
    {
        e.Title = dto.Title;
        e.Description = dto.Description;
        e.BoletasNumber = dto.BoletasNumber;
        e.BoletasValue = dto.BoletasValue;
        e.LotteryDate = dto.LotteryDate;
        e.LotteryNumber = dto.LotteryNumber;
        e.Jackpot = dto.Jackpot;
    }
}
