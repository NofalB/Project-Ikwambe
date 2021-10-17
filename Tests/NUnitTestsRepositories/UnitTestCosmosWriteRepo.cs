using Domain;
using Infrastructure.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace NUnitTestsRepositories
{
    public class UnitTestCosmosWriteRepocs
    {
        private List<Donation> _MockLstDives;
        /*private Mock<ICosmosWriteRepository> _DiveMock;
        private ICosmosWriteRepository _DiveRepo;*/

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}