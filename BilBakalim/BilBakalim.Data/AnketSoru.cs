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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AnketSoru()
        {
            AnketOturum = new HashSet<AnketOturum>();
        }

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

        public int? SinifID { get; set; }

        public int? Sure { get; set; }

        public DateTime OlusturmaTarihi { get; set; }

        public virtual Anket Anket { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnketOturum> AnketOturum { get; set; }

        public virtual Resim Resim { get; set; }
    }
}
