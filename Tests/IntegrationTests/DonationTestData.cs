using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class DonationTestData
    {
        public static IEnumerable<object[]> Donations()
        {
            return new List<object[]>{
                new object[] { new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "1Y7311651B552625V", 4000) },
                new object[] { new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "2Y7311651B552625X", 599) },
                new object[] { new Donation(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "3Y7311651B552625W", 200) }
            };
        }
    }
}
