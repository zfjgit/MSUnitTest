using System.Configuration;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSUnitTestProject.UserInfoUpdate;

namespace MSUnitTestProject.Tests.UserInfoUpdate
{
    [TestClass]
    public class UserInfoUpdateUnitTest
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserInfoUpdateUnitTest));

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            ConfigurationManager.AppSettings["ConnectionString"] = "Initial Catalog=master;Server=.;";
            ConfigurationManager.AppSettings["DBUser"] = "sa";
            ConfigurationManager.AppSettings["DBPassword"] = "sasa";

            log.Info("Class Initialize...");
        }

        [TestInitialize]
        public void BeforeTest()
        {
            log.Info("Before Test...");
        }

        [TestCleanup]
        public void AfterTest()
        {
            log.Info("After Test...");
        }

        [TestMethod]
        public void TestUserInfoUpdate()
        {
            UserInfoExcelUpdater.Update();
        }

        [TestMethod]
        public void TestLog()
        {
            log.Info("Test Log...");
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            log.Info("Class Cleanup...");
        }
    }
}