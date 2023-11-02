using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBRmats.Core.Entities
{
    public class MaterialTag
    {
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        [ForeignKey("Tag")]
        public int TagsId { get; set; }
        public Tag Tag { get; set; }

    }
}
