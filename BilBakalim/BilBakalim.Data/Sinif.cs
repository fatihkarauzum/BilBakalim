namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sinif")]
    public partial class Sinif
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sinif()
        {
            Favori = new HashSet<Favori>();
            Rapor = new HashSet<Rapor>();
            Sorular = new HashSet<Sorular>();
        }

        public int ID { get; set; }

        [StringLength(75)]
        public string Ad { get; set; }

        [StringLength(500)]
        public string Aciklama { get; set; }

        public bool? Gorunurluk { get; set; }

        public DateTime? OlusturmaTarihi { get; set; }

        public int? GoruntulenmeSayisi { get; set; }

        public int? LisanID { get; set; }

        public int? ResimID { get; set; }

        [StringLength(500)]
        public string VideoUrl { get; set; }

        public int? KullaniciID { get; set; }

        public int? SinifKategoriID { get; set; }

        public virtual Dİl Dİl { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favori> Favori { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rapor> Rapor { get; set; }

        public virtual Resim Resim { get; set; }

        public virtual SinifKategori SinifKategori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sorular> Sorular { get; set; }
    }
}
