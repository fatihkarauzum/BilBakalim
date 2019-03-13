namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Takip")]
    public partial class Takip
    {
        public int ID { get; set; }

        public int? TakipEdenID { get; set; }

        public int? TakipEdilenID { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Kullanici Kullanici1 { get; set; }
    }
}
