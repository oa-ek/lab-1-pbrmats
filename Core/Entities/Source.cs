using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBRmats.Core.Entities
{
    public class Source : Entity
    {
        public string Title { get; set; } = string.Empty;
    }
}
