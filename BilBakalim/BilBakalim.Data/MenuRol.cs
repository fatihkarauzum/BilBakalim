using BilBakalim.Data.Interfaces;

namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MenuRol")]
    public partial class MenuRol : Entity
    {
        public int ID { get; set; }

        public int? RolId { get; set; }

        public int? MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual Rol Rol { get; set; }
    }
}
