
#region Author and License Information
/* Tls Helper
 * version 1.0
 * written by Steven Hunt
 * released under the MIT License. */
#endregion

 using System;

namespace StevenHunt.Tls.Providers
{
	/// <summary>
	/// Allows TlsHelper to use HTTP Context in the case of an ASP .Net application.
	/// </summary>
	public class HttpContextProvider<T>: StevenHunt.Tls.ITlsProvider<T>
	{		
		public bool IsAvailable
		{
			get
			{
				return System.Web.HttpContext.Current != null;
			}
		}
		
		public T GetValue(string identifier)
		{
			return (T)System.Web.HttpContext.Current.Items[identifier];
		}
		
		public void SetValue(string identifier, T value)
		{
			System.Web.HttpContext.Current.Items[identifier] = value;
		}
		
		public bool HasValue(string identifier)
		{
			return System.Web.HttpContext.Current.Items.Contains(identifier);
		}
	}
}
