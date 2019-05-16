namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnketSoru")]
    public partial class AnketSoru
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Soru { get; set; }

        public int? MedyaID { get; set; }

        [StringLength(500)]
        public string Cevap1 { get; set; }

        [StringLength(500)]
        public string Cevap2 { get; set; }

        [StringLength(500)]
        public string Cevap3 { get; set; }

        [StringLength(500)]
        public string Cevap4 { get; set; }

        public int? AnketID { get; set; }

        public virtual Anket Anket { get; set; }
    }
}
