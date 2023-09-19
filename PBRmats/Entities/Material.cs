using PBRmats.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBRmatsCore.Entities
{
    public class Material
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string AvgColor { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<PBRmats.Core.Entities.License> Licenses { get; set; }
        public virtual ICollection<Source> Sources { get; set; }
    }
}
