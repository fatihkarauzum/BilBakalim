namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnketOturum")]
    public partial class AnketOturum
    {
        public int ID { get; set; }

        public int? Pin { get; set; }

        public int? Cevap1 { get; set; }

        public int? Cevap2 { get; set; }

        public int? Cevap3 { get; set; }

        public int? Cevap4 { get; set; }

        public int? SoruID { get; set; }

        public virtual AnketSoru AnketSoru { get; set; }
    }
}
