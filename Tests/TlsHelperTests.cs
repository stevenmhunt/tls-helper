
#region Author and License Information
/* Tls Helper
 * version 1.0
 * written by Steven Hunt
 * released under the MIT License. */
#endregion

using System;
using System.IO;

using System.Threading;
using System.Web;
using System.Web.Hosting;

using NUnit.Framework;

using StevenHunt.Tls;

namespace StevenHunt.Tls
{
	[TestFixture]
	public class TlsHelperTests
	{
		[Test]
		public void ThreadStaticProviderTest()
		{
			
			//make sure the http context is cleared.
			HttpContext.Current = null;

			var var1 = new TlsHelper<string>("var1", () => "initial");

			Assert.AreEqual(typeof(Providers.ThreadStaticProvider<string>), var1.Provider.GetType());
			
			Thread t1 = new Thread(delegate() {
				var1.Value = "thread 1";
				Assert.AreEqual(var1.Value, "thread 1");
			});

			Thread t2 = new Thread(delegate() {
				var1.Value = "thread 2";
				Assert.AreEqual(var1.Value, "thread 2");
			});

			Thread t3 = new Thread(delegate() {
				var1.Value = "thread 3";
				Assert.AreEqual(var1.Value, "thread 3");
			});
			
			t1.Start();
			t2.Start();
			t3.Start();
			
			t1.Join();
			t2.Join();
			t3.Join();
			
			Assert.AreEqual(var1.Value, "initial");
		}
		
		[Test]
		public void HttpContextProviderTest()
		{
			//create a fake http request.
			SimpleWorkerRequest request = new SimpleWorkerRequest("","","", null, new StringWriter());
			HttpContext context = new HttpContext(request);
			HttpContext.Current = context;
			
			var var2 = new TlsHelper<string>("var2", () => "initial");

			//test that the TLS helper picked the right provider and is storing values correctly.
			Assert.AreEqual(typeof(Providers.HttpContextProvider<string>), var2.Provider.GetType());
			Assert.AreEqual(var2.Value, "initial");			
			
			var2.Value = "New Value";

			Assert.AreEqual(var2.Value, "New Value");			
		}
	}
}