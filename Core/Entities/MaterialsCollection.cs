using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBRmats.Core.Entities
{
    public class MaterialsCollection : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string CardColor { get; set; } = string.Empty;

        [Display(Name = "User")]
        [ForeignKey("AppUser")]
        public Guid AppUserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public ICollection<MaterialMaterialsCollection> MaterialMaterialsCollection { get; set; }
    }
}
