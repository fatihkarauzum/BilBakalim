using BilBakalim.Data.Interfaces;

namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Resim")]
    public partial class Resim : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resim()
        {
            Sinif = new HashSet<Sinif>();
            SinifKategori = new HashSet<SinifKategori>();
        }

        public int ID { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        public int? ResimKategoriID { get; set; }

        public virtual ResimKategori ResimKategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sinif> Sinif { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinifKategori> SinifKategori { get; set; }
    }
}
