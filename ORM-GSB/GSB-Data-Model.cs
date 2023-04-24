using Model;
using System.Data.Entity;

namespace ORM_GSB
{
    public partial class GSB_Data_Model : DbContext
    {
        public GSB_Data_Model()
            : base("name=GSBDataModel")
        {
        }
        // Départements
        public virtual DbSet<Departement> Departement { get; set; }

        //Médecins
        public virtual DbSet<Medecin> Medecin { get; set; }

        //Users
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departement>()
                .Property(e => e.dep_name)
                .IsUnicode(false);

            modelBuilder.Entity<Departement>()
                .Property(e => e.region_name)
                .IsUnicode(false);

            modelBuilder.Entity<Departement>()
                .HasMany(e => e.Medecin)
                .WithRequired(e => e.Departement)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Medecin>()
                .Property(e => e.prenom)
                .IsUnicode(false);

            modelBuilder.Entity<Medecin>()
                .Property(e => e.nom)
                .IsUnicode(false);

            modelBuilder.Entity<Medecin>()
                .Property(e => e.adresse)
                .IsUnicode(false);

            modelBuilder.Entity<Medecin>()
                .Property(e => e.telephone)
                .IsUnicode(false);

            modelBuilder.Entity<Medecin>()
                .Property(e => e.specialite)
                .IsUnicode(false);
        }
    }
}
