using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{


    [Table("Medecin")]
    public partial class Medecin
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string prenom { get; set; }

        [Required]
        [StringLength(50)]
        public string nom { get; set; }

        [Required]
        [StringLength(250)]
        public string adresse { get; set; }

        [Required]
        [StringLength(50)]
        public string telephone { get; set; }

        [StringLength(150)]
        public string specialite { get; set; }

        public int num_dep { get; set; }

        public virtual Departement Departement { get; set; }
    }
}
