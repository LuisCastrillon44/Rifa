namespace Application.DTOs.Boletas;

public record BoletaDto(
    long Id,
    long TalonarioId,
    int Number,
    string? BuyerName,
    string? BuyerPhone,
    string? BuyerAddress,
    bool Sold);

public record CreateBoletaDto(
    long TalonarioId,
    int Number,
    string? BuyerName,
    string? BuyerPhone,
    string? BuyerAddress,
    bool Sold);

public record UpdateBoletaDto(
    string? BuyerName,
    string? BuyerPhone,
    string? BuyerAddress,
    bool Sold);
