using Biomet.Models.Entities;
using Biomet.Models.PayReceipt;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biomet.Models.Persistence
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class BiometContext : DbContext
    {
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<DayLog> DayLogs { get; set; }

        public BiometContext()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<BiometContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PayCheck>().Property(p => p._Deductions).HasColumnName("Deductions");
            modelBuilder.Entity<PayCheck>().Property(p => p._Additions).HasColumnName("Additions");
            modelBuilder.Entity<PayCheck>().Property(p => p._Premiums).HasColumnName("Premiums");
        }
    }
}
