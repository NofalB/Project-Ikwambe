using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.Object
{
    public class UserTest
    {
		public Guid UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Subscription { get; set; }

        public UserTest()
        {

        }
	}
}
