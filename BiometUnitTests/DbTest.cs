using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biomet.Models.Persistence;
using Biomet.Models.Entities;

namespace BiometUnitTests
{
    [TestClass]
    public class DbTest
    {
        [TestCleanup]
        public void CleanUp()
        {
            using (var db = new BiometContext())
            {
                db.Employees.RemoveRange(db.Employees);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void CanAccessDb()
        {
            using (var db = new BiometContext())
            {

                db.Employees.Add(new SalariedEmployee
                {
                    MonthlySalary = 14000
                });
                db.SaveChanges();
            }

            using (var db = new BiometContext())
            {
                Assert.IsTrue(db.Employees.ToList().Any());
            }
        }
    }
}
