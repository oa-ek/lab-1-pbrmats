using System.ComponentModel.DataAnnotations.Schema;

namespace PBRmats.Core.Entities
{
    public class MaterialMaterialsCollection : Entity
    {
        [ForeignKey("MaterialsCollection")]
        public int MaterialsCollectionId { get; set; }
        public MaterialsCollection MaterialsCollection { get; set; }

        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
    }
}
