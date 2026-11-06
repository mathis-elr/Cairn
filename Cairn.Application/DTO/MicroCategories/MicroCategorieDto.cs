using System;
namespace Cairn.Application.DTO.MicroCategories
{
    public class MicroCategorieDto
    {
        public int Id { get; set; }

        public required string Libelle { get; set; }

        public byte Ordre { get; set; }
    }
}
