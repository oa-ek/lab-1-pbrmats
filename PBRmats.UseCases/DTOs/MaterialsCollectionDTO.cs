using PBRmats.Core.Entities;

namespace PBRmats.Application.DTOs
{
    public class MaterialsCollection : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string CardColor { get; set; } = string.Empty;
        public string AppUserId { get; set; }
    }
}
