using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{


    [Table("Departement")]
    public partial class Departement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Departement()
        {
            Medecin = new HashSet<Medecin>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num_dep { get; set; }

        [StringLength(100)]
        public string dep_name { get; set; }

        [StringLength(100)]
        public string region_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Medecin> Medecin { get; set; }
    }
}
