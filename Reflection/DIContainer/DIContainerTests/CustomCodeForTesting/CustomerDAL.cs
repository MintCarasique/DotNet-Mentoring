using DIContainer.Attributes;

namespace DIContainerTests.CustomCodeForTesting
{
	[Export(typeof(ICustomerDAL))]
	public class CustomerDAL : ICustomerDAL
	{
		public CustomerDAL()
		{
		}
	}
}
