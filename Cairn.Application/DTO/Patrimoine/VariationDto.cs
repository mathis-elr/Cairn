using Cairn.Domain.Enums;

namespace Cairn.Application.DTO.Patrimoine;

public class VariationDto
{
    public required Periode Periode { get; set; }

    public decimal VariationPourcentage { get; set; }

    public decimal VariationValeure { get; set; }
}