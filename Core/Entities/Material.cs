using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBRmats.Core.Entities
{
    public class Material : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string AvgColor { get; set; } = string.Empty;
        public string AvgSpecularColor { get; set; } = string.Empty;
        public float AvgMetallic { get; set; }
        public float AvgIOR { get; set; }
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [Display(Name = "License")]
        [ForeignKey("License")]
        public int LicenseId { get; set; }
        public virtual License License { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public ICollection<MaterialTag> MaterialTags { get; set; }
        public ICollection<MaterialMaterialsCollection> MaterialMaterialsCollection { get; set; }
    }
}
