using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBRmats.Core.Entities
{
    public class Tag : Entity
    {
        public string Title { get; set; } = string.Empty; 
        public ICollection<MaterialTag> MaterialTags { get; set; }
    }
}
