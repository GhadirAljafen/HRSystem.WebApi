using HRDataLayer.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using static HRDataLayer.Migrations.Configuration;

namespace HRDataLayer
{
    public class HRContext : DbContext
    {

        public HRContext() : base("name=connnectionString") {

           // Database.SetInitializer<HRContext>(new CreateDatabaseIfNotExists<HRContext>());
           Database.SetInitializer(new HRDBInitializer());
        }
      
        
        public DbSet<User> Users { set; get; }
        public DbSet<Vacation> Vacations { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            EntityTypeConfiguration<User> user = modelBuilder.Entity<User>();
            EntityTypeConfiguration<Vacation> vacation = modelBuilder.Entity<Vacation>();
        
            user.Property(p => p.Name).IsRequired().HasMaxLength(50);
            user.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            user.Property(p => p.Username).IsRequired().HasMaxLength(50).HasColumnType("VARCHAR");
            user.Property(p => p.Email).IsRequired().HasMaxLength(50).HasColumnType("VARCHAR");
            user.Property(p => p.Mobile).IsRequired().HasMaxLength(15).HasColumnType("VARCHAR");
            user.Property(p => p.JobTitle).HasMaxLength(50);
            user.Property(p => p.ManagerID).IsOptional();

            vacation.Property(p => p.Attatchment).IsOptional();
            vacation.Property(p => p.Description).HasMaxLength(3000).IsOptional();
            vacation.Property(p => p.EmployeeID).IsRequired();
            vacation.Property(p => p.RejectionReason).IsOptional().HasMaxLength(300);
            vacation.HasRequired<User>(v => v.User).WithMany(v => v.Vacations).HasForeignKey(v => v.EmployeeID).WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
           
        }
    }
}
