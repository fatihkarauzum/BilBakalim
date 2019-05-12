namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Favori")]
    public partial class Favori
    {
        public int ID { get; set; }

        public int? KullaniciID { get; set; }

        public int? SinifID { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Sinif Sinif { get; set; }
    }
}
