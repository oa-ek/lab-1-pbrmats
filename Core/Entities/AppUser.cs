using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PBRmats.Core.Entities
{
    public class AppUser : Entity
    {
        public string Id { get; set; }
        public virtual ICollection<MaterialsCollection> MaterialsCollections { get; set; }
    }
}
