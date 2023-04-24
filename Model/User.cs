namespace Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("User")]
    public partial class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Pseudo { get; set; }

        [Required]
        [StringLength(10)]
        public string MotDePasse { get; set; }
    }
}
