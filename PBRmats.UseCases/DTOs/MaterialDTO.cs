using PBRmats.Core.Entities;

namespace PBRmats.Application.DTOs
{
    public class MaterialDTO : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string AvgColor { get; set; } = string.Empty;
        public string AvgSpecularColor { get; set; } = string.Empty;
        public float AvgMetallic { get; set; }
        public float AvgIOR { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int CategoryId { get; set; }
        public int LicenseId { get; set; }
        public string ImageUrl { get; set; }
        public string ZipFileUrl { get; set; }
    }
}
