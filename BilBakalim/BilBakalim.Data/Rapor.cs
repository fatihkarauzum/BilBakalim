using BilBakalim.Data.Interfaces;

namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rapor")]
    public partial class Rapor : Entity
    {
        public int ID { get; set; }

        public DateTime? OyunZaman { get; set; }

        public int? SınıfID { get; set; }

        public int? KullaniciID { get; set; }

        public int? KisiSayisi { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Sinif Sinif { get; set; }
    }
}
