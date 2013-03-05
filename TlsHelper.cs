
#region Author and License Information
/* Tls Helper
 * version 1.0
 * written by Steven Hunt
 * released under the MIT License. */
#endregion
 
using System;
using System.Collections.Generic;

namespace StevenHunt.Tls
{
	/// <summary>
	/// Defines a new TLS helper object, which represents a value of type T to store in Thread Local Storage.
	/// </summary>
	public class TlsHelper<T>
	{
		
		/// <summary>
		/// A list of registered TLS providers.
		/// </summary>
		private static List<ITlsProvider<T>> Providers = new List<ITlsProvider<T>>();
		
		/// <summary>
		/// Static constructor. Registers the pre-built TLS providers in the library.
		/// </summary>
		static TlsHelper()
		{
			AddProvider(new Providers.ThreadStaticProvider<T>());
			AddProvider(new Providers.HttpContextProvider<T>());
		}

		/// <summary>
		/// Allows an application to register a custom provider to consider when deciding which provider to use.
		/// </summary>
		/// <param name="provider">The provider implementing ITlsProvider to register.</param>
		/// <remarks>
		/// Note that providers are considered in reverse order in which they are registered. For instance, the
		/// ThreadStaticProvider is the first provider registered, but the last one considered for use.
		/// </remarks>
		public static void AddProvider(ITlsProvider<T> provider)
		{
			foreach (ITlsProvider<T> p in Providers)
			{
				if (p.GetType().Equals(provider.GetType()))
					return;
			}
			
			Providers.Insert(0, provider);
		}

		/// <summary>
		/// Iterates through all available provider and chooses the first one that is available for use based on the
		/// current program environment.
		/// </summary>
		protected static ITlsProvider<T> chooseProvider()
		{
			foreach (ITlsProvider<T> p in Providers)
			{
				if (p.IsAvailable)
					return p;
			}
			
			return null;
		}

		/// <summary>
		/// The provider for this tls helper instance.
		/// </summary>
		public ITlsProvider<T> Provider { get; private set; }
		
		/// <summary>
		/// The identifier set to this TLS helper to uniquely identify it.
		/// </summary>
		public string Identifier { get; private set; }
		
		/// <summary>
		/// The initializer function used to lazy load the value for this TLS helper.
		/// </summary>		
		private Func<T> _initializer = null;

		/// <summary>
		/// Constructor. Creates a TLS helper object with the specified identifier and initializer function.
		/// </summary>		
		public TlsHelper(string identifier, Func<T> initializer)
		{
			Provider = chooseProvider();
			Identifier = identifier;
			_initializer = initializer;
		}
		
		/// <summary>
		/// Indicates whether or not the current TLS helper instance value has been initialized.
		/// </summary>
		public bool HasValue
		{
			get
			{
				return Provider.HasValue(Identifier);
			}
		}
		
		/// <summary>
		/// Gets or sets the value being stored in TLS.
		/// </summary>
		public T Value
		{
			get
			{
				//if the value hasn't been set, use the initializer function to set its value first.
				if (!Provider.HasValue(Identifier))
				{
					Provider.SetValue(Identifier, _initializer.Invoke());
				}
				
				return Provider.GetValue(Identifier);
			}
			set
			{
				Provider.SetValue(Identifier, value);
			}
		}		
	}
}