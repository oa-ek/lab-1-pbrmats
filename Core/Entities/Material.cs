using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PBRmats.Core.Entities
{
    public class Material : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string AvgColor { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        public virtual Category Category { get; set; }
        public virtual License License { get; set; }
        public virtual ICollection<Source> Sources { get; set; }
    }
}
