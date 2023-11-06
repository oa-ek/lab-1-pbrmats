using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBRmats.Core.Entities
{
    public class AppUser
    {
        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<MaterialsCollection> MaterialsCollections { get; set; }
    }
}
