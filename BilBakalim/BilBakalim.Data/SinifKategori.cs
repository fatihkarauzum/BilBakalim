namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinifKategori")]
    public partial class SinifKategori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SinifKategori()
        {
            Sinif = new HashSet<Sinif>();
        }

        public int ID { get; set; }

        [StringLength(75)]
        public string KategoriAdi { get; set; }

        public int? ResimID { get; set; }

        public bool? Aktif { get; set; }

        public virtual Resim Resim { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sinif> Sinif { get; set; }
    }
}
