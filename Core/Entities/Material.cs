using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBRmats.Core.Entities
{
    public class Material : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string AvgColor { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public virtual Category Category { get; set; }
        [Display(Name = "License")]
        [ForeignKey("License")]
        public int LicenseId { get; set; }
        public virtual License License { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }
}
