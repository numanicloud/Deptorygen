﻿// <autogenerated />
using System;
using System.Collections.Generic;

namespace UseDeptorygen.Samples.Capture
{
	internal partial class BaseFactory : IBaseFactory
		, IDisposable
	{
		private Service? _ResolveServiceCache;
		private CaptureFactory? _ResolveCaptureFactoryCache;

		public BaseFactory()
		{
		}

		public Service ResolveService()
		{
			return _ResolveServiceCache ??= new Service();
		}

		public ICaptureFactory ResolveCaptureFactory(Service2 service2)
		{
			return _ResolveCaptureFactoryCache ??= new CaptureFactory(service2, this);
		}

		public void Dispose()
		{
			_ResolveCaptureFactoryCache?.Dispose();
		}
	}
}