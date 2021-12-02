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
    using System.ComponentModel.DataAnnotations;

    public partial class Asiakkaat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Asiakkaat()
        {
            this.Rekisteroitymiset = new HashSet<Rekisteroitymiset>();
        }
    
        public int AsiakasID { get; set; }
        public string Nimi { get; set; }
        public string Sposti { get; set; }
        public string Osoite { get; set; }
        public Nullable<int> PostiID { get; set; }
        public Nullable<System.DateTime> RekisterointiPvm { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public virtual Postitoimipaikat Postitoimipaikat { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rekisteroitymiset> Rekisteroitymiset { get; set; }
        public string PostitoimipaikatPostinumero => $"{Osoite}, {Postitoimipaikat.Postinumero}, {Postitoimipaikat.Postitoimipaikka}";

    }
}
