
using System;
using System.Diagnostics;
using Biomet.Extentions;
using Biomet.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BiometUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void FridayMinus4IsMonday()
        {
            var dayOnFriday = new DateTime(2018, 2, 9);
            Assert.IsTrue(dayOnFriday.AddDays(-4).DayOfWeek == DayOfWeek.Monday);
        }

        [TestMethod]
        public void CanDetermineWeekNumber()
        {
            Assert.AreEqual(1, new DateTime(2018, 1, 5).GetWeekOfMonth());
            Assert.AreEqual(2, new DateTime(2018, 1, 12).GetWeekOfMonth());
            Assert.AreEqual(3, new DateTime(2018, 1, 19).GetWeekOfMonth());
            Assert.AreEqual(4, new DateTime(2018, 1, 26).GetWeekOfMonth());

            Debug.WriteLine("Week number of 02 Feb 2018 is: " + new DateTime(2018, 2, 2).GetWeekOfMonth());
        }

       
    }
}
