using Xunit;
using API.Controllers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API.Models;
using System.Collections.Generic;

namespace API.UnitTests.Controllers
{
    public class EmployeesControllerTests
    {
        private DbContextOptions<EmployeesContext> ContextOptions;

        private EmployeesController controller;
        public EmployeesControllerTests()
        {
            ContextOptions = new DbContextOptionsBuilder<EmployeesContext>()
            .UseInMemoryDatabase("EmployeeDB")
            .Options;

            SeedDb();
            controller = new EmployeesController(new EmployeesContext(ContextOptions));
        }

        private void SeedDb()
        {
            using (var context = new EmployeesContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var emp1 = new Employee();
                emp1.FirstName = "User1";
                emp1.FirstName = "MName";
                emp1.LastName = "Doeuser";

                var emp2 = new Employee();
                emp1.FirstName = "Jane";
                emp1.LastName = "Doe";

                context.AddRange(emp1, emp2);
                context.SaveChanges();
            }
        }

        [Fact]
        public void Can_Get_Items()
        {
            var items = controller.GetEmployees().Result.Value.ToList<Employee>();

            Assert.Equal(2, items.Count());
        }
    }
}