using System.Data.Entity.ModelConfiguration;

namespace EmployeeTracker.Data.Persistance.EntityConfiguration
{
    public class ArrivalConfiguration : EntityTypeConfiguration<Arrival>
    {
        public ArrivalConfiguration()
        {
            Property(arrival => arrival.EmployeeId).IsRequired();
        }
    }
}
