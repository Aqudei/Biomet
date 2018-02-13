using System;
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
        public void CanCreateCorrectEmployeeType()
        {

        }
    }
}
