﻿// <autogenerated />
using System;
using System.Collections.Generic;

namespace UseDeptorygen.Samples.Transient
{
	internal partial class Factory : IFactory
		, IDisposable
	{
		private Client? _ResolveClientCache;

		public Factory()
		{
		}

		public Service ResolveServiceAsTransient()
		{
			return new Service();
		}

		public Client ResolveClient()
		{
			return _ResolveClientCache ??= new Client(ResolveServiceAsTransient(), ResolveServiceAsTransient());
		}

		public void Dispose()
		{
		}
	}
}