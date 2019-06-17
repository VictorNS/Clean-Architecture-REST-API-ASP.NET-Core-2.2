using System.Collections.Generic;

namespace Testflight.Domain.Entities
{
	public class Customer
	{
		public Customer()
		{
			Orders = new HashSet<Order>();
		}

		public int CustomerId { get; set; }
		public string CompanyName { get; set; }
		public string Address { get; set; }

		public ICollection<Order> Orders { get; private set; }
	}
}
