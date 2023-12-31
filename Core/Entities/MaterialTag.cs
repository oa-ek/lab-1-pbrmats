﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PBRmats.Core.Entities
{
    public class MaterialTag : Entity
    {
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        [ForeignKey("Tag")]
        public int TagsId { get; set; }
        public Tag Tag { get; set; }

    }
}
