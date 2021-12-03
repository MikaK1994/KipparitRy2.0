using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KipparitRy2._0.Models
{
    public class Rekisteroinutasiakas
    {
        public int AsiakasID { get; set; }
        public string Nimi { get; set; }
        public string Sposti { get; set; }
        public string Osoite { get; set; }
        public Nullable<int> PostiID { get; set; }
        public Nullable<int> TilaisuusID { get; set; }
        public bool EhdotBox { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> RekisterointiPvm { get; set; }
        public virtual Postitoimipaikat Postitoimipaikat { get; set; }
        public virtual Tilaisuudet Tilaisuudet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tilaisuudet> Tilaisuudet1 { get; set; }

    }
}