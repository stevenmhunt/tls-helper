
#region Author and License Information
/* Tls Helper
 * version 1.0
 * written by Steven Hunt
 * released under the MIT License. */
#endregion

using System;

namespace StevenHunt.Tls
{
	/// <summary>
	/// Provides a common interface for providers implementing TLS for the TLS helper.
	/// </summary>
	public interface ITlsProvider<T>
	{
		/// <summary>
		/// Returns whether or not this provider is available in the current environment.
		/// </summary>
		bool IsAvailable { get; }

		/// <summary>
		/// Retrieves a value by the specified name.
		/// </summary>
		T GetValue(string identifier);

		/// <summary>
		/// Sets a value by the specified name.
		/// </summary>
		void SetValue(string identifier, T value);
		
		/// <summary>
		/// Returns whether there is a value by the specified name stored in the collection.
		/// </summary>
		bool HasValue(string identifier);
	}
}
