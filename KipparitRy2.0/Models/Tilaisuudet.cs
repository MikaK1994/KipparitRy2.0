//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KipparitRy2._0.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tilaisuudet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tilaisuudet()
        {
            this.Rekisteroitymiset = new HashSet<Rekisteroitymiset>();
        }
    
        public int TilaisuusID { get; set; }
        public string Nimi { get; set; }
        public string Jarjestaja { get; set; }
        public Nullable<System.DateTime> Pvm { get; set; }
        public Nullable<int> MaxMaara { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rekisteroitymiset> Rekisteroitymiset { get; set; }
    }
}
