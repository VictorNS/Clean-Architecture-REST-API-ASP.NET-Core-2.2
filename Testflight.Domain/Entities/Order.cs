using System;
using System.Collections.Generic;

namespace Testflight.Domain.Entities
{
	public class Order
	{
		public Order()
		{
			OrderDetails = new HashSet<OrderDetail>();
		}

		public int OrderId { get; set; }
		public int CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime? ShippedDate { get; set; }

		public Customer Customer { get; set; }
		public ICollection<OrderDetail> OrderDetails { get; private set; }
	}
}
