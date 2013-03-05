
#region Author and License Information
/* Tls Helper
 * version 1.0
 * written by Steven Hunt
 * released under the MIT License. */
#endregion

 using System;
using System.Collections.Generic;

namespace StevenHunt.Tls.Providers
{
	/// <summary>
	/// Default provider; uses a thread-static object to track values.
	/// </summary>
	public class ThreadStaticProvider<T>: StevenHunt.Tls.ITlsProvider<T>
	{
		[ThreadStatic]
		private static Dictionary<string, T> _values = null;
		
		public bool IsAvailable
		{
			get
			{
				return true;
			}
		}
		
		public T GetValue(string identifier)
		{
			if (_values == null) _values = new Dictionary<string, T>();
			return _values[identifier];
		}
		
		public void SetValue(string identifier, T value)
		{
			if (_values == null) _values = new Dictionary<string, T>();			
			_values[identifier] = value;
		}
		
		public bool HasValue(string identifier)
		{
			if (_values == null) _values = new Dictionary<string, T>();
			return _values.ContainsKey(identifier);
		}
	}
}
