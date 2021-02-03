using Xunit;
using API.Controllers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System;

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
            // Use NullLogger to replace NLog
            controller = new EmployeesController(
                new EmployeesContext(ContextOptions),
                new NullLogger<EmployeesController>()
            );
        }

        private void SeedDb()
        {
            using (var context = new EmployeesContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var emp1 = new Employee();
                emp1.FirstName = "User1";
                emp1.MiddleName = "MName";
                emp1.LastName = "Doeuser";
                emp1.HireDate = Convert.ToDateTime("2020-04-15");
                emp1.UserName = "user1";

                var emp2 = new Employee();
                emp2.FirstName = "Jane";
                emp2.LastName = "Doe";
                emp2.HireDate = Convert.ToDateTime("2021-01-18");
                emp2.UserName = "user2";

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

        [Fact]
        public void Seed_Data_Correct()
        {
            var items = controller.GetEmployees().Result.Value.ToList<Employee>();

            Assert.Equal("user2",items[1].UserName);
            Assert.Equal("user1",items[0].UserName);
            Assert.Equal(Convert.ToDateTime("2020-04-15"),items[0].HireDate);
            Assert.Equal(Convert.ToDateTime("2021-01-18"),items[1].HireDate);
        }

        [Fact]
        public void Update_User_Data()
        {
            var employee = controller.GetEmployees(1).Result.Value;
            employee.MiddleName = "Tnok";

            controller.PutEmployees(employee.Id,employee).Wait();

            var employeeUpdated = controller.GetEmployees(1).Result.Value;

            Assert.Equal("Tnok",employeeUpdated.MiddleName);
            Assert.Equal(employee.Id,employeeUpdated.Id);
            Assert.Equal(employee.FirstName,employeeUpdated.FirstName);
        }

        [Fact]
        public void Add_New_User()
        {
            var employee = new Employee();
            employee.FirstName="New";
            employee.LastName="User";
            employee.HireDate=Convert.ToDateTime("2021-02-2");
            employee.UserName = $"{employee.FirstName}{employee.LastName}";

            controller.PostEmployees(employee).Wait();

            var items = controller.GetEmployees().Result.Value.ToList<Employee>();
            var tEmp = controller.GetEmployees().Result.Value.ToList()
                .Where(s => s.Id == 3)
                .FirstOrDefault();
            Assert.Equal(3, items.Count());
            Assert.Equal("New",tEmp.FirstName);
        }
    }
}