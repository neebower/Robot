using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Robot
{
    [TestFixture]
    public class TestBase
    {
        protected Robot Robot;

        [SetUp]
        public void SetUp()
        {
            Robot = new Robot();
        }

        protected void OutputShouldBe(List<string> expected, List<string> actual)
        {
            if (expected == null || actual == null)
                Assert.Fail();

            if (expected.Count != actual.Count)
                Assert.Fail();

            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        protected void RunTest(Action test, string testName)
        {
            Robot = new Robot();
            try
            {
                test();
            }
            catch (Exception)
            {
                throw new Exception($"Test '{testName}' failed.");
            }
        }
    }
}
