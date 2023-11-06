namespace PBRmats.Core.Entities
{
    public class Tag : Entity
    {
        public string Title { get; set; } = string.Empty; 
        public ICollection<MaterialTag> MaterialTags { get; set; }
    }
}
