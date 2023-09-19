using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBRmatsCore.Entities;

namespace PBRmats.Core.Entities
{
    public class MaterialsCollection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public User ParentUser { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}
