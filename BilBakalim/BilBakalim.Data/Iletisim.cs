namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Iletisim")]
    public partial class Iletisim
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Isim { get; set; }

        [StringLength(50)]
        public string Eposta { get; set; }

        [StringLength(50)]
        public string Telefon { get; set; }

        [StringLength(500)]
        public string Icerik { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Tarih { get; set; }
    }
}
