using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class EmployeesContext : DbContext
    {
        public EmployeesContext(DbContextOptions<EmployeesContext> options):base(options)
        {

        }

        public DbSet<Employee> Employees{get;set;}
    }
}