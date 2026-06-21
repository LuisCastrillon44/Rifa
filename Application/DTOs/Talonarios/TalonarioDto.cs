namespace Application.DTOs.Talonarios;

public record TalonarioDto(
    long Id,
    long UserId,
    string Title,
    string? Description,
    int BoletasNumber,
    decimal BoletasValue,
    DateTime LotteryDate,
    string? LotteryNumber,
    string Jackpot);

public record CreateTalonarioDto(
    long UserId,
    string Title,
    string? Description,
    int BoletasNumber,
    decimal BoletasValue,
    DateTime LotteryDate,
    string? LotteryNumber,
    string Jackpot);

public record UpdateTalonarioDto(
    string Title,
    string? Description,
    int BoletasNumber,
    decimal BoletasValue,
    DateTime LotteryDate,
    string? LotteryNumber,
    string Jackpot);
