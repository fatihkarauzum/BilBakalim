namespace BilBakalim.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Anket")]
    public partial class Anket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Anket()
        {
            AnketSoru = new HashSet<AnketSoru>();
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

        public int? KullaniciID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnketSoru> AnketSoru { get; set; }
    }
}
