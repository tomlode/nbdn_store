using System.Data;
using NUnit.Framework;

namespace nothinbutdotnetstore.specs.dataaccesslayer
{
    public class DatabaseGatewaySpecs
    {
        public class when_executing_a_query
        {
            [Test]
            public void _should_return_a_datatable()
            {
                Assert.IsInstanceOf(typeof(DataTable), DatabaseGateway.executequery("SELECT * FROM Customers"));
            }
        }
    }
}