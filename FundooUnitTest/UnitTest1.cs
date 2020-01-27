using NUnit.Framework;

namespace FundooUnitTest
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UserLogin()
        {
            UserAccountUnitTest userAccount = new UserAccountUnitTest();
            Assert.AreEqual(false, userAccount.Login("", ""));
        }
    }
}