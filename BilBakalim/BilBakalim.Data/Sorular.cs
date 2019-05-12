namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sorular")]
    public partial class Sorular
    {
        public int ID { get; set; }

        [StringLength(500)]
        public string Soru { get; set; }

        public int? Sure { get; set; }

        public bool? Odul { get; set; }

        public int? MedyaID { get; set; }

        [StringLength(500)]
        public string Cevap1 { get; set; }

        [StringLength(500)]
        public string Cevap2 { get; set; }

        [StringLength(500)]
        public string Cevap3 { get; set; }

        [StringLength(500)]
        public string Cevap4 { get; set; }

        [StringLength(6)]
        public string DogruCevap { get; set; }

        public int? SinifID { get; set; }

        public virtual Sinif Sinif { get; set; }
    }
}
