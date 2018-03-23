using System;
using Biomet.Models.Entities;
using Biomet.Models.Persistence;
using Biomet.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BiometUnitTests
{
    [TestClass]
    public class DTRTest
    {
        [TestInitialize]
        public void Cleanup()
        {
            using (var context = new BiometContext())
            {
                context.Employees.RemoveRange(context.Employees);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void TestMethod1()
        {

            using (var context = new BiometContext())
            {
                context.Employees.Add(new SalariedEmployee
                {
                    EmployeeNumber = "888",
                    MonthlySalary = 30000,
                    FirstName = "Archie"
                });
                context.SaveChanges();

                DTRRepository dTRRepository = new DTRRepository(context);
                dTRRepository.Get("888", DateTime.Now.Date);
            }
        }
    }
}
