﻿// <autogenerated />
using System;
using System.Collections.Generic;

namespace UseDeptorygen.Samples.Capture
{
	internal partial class CaptureFactory : ICaptureFactory
		, IDisposable
	{
		private readonly Service2 _service2;

		public IBaseFactory BaseFactory { get; }

		private Client? _ResolveClientCache;

		public CaptureFactory(Service2 service2, IBaseFactory baseFactory)
		{
			_service2 = service2;
			BaseFactory = baseFactory;
		}

		public Client ResolveClient()
		{
			return _ResolveClientCache ??= new Client(BaseFactory.ResolveService(), _service2);
		}

		public void Dispose()
		{
		}
	}
}