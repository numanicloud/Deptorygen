﻿// <autogenerated />
using System;

namespace Deprovgen.Try
{
    internal partial class ServiceLocator : IServiceLocator
    {
        private readonly Service _service;
        private readonly Service2 _service2;
        private readonly Service3 _service3;

        private Hoge? _resolveHogeCache;
        private Piyo? _resolvePiyoCache;
        private Fuga? _resolveFugaCache;

        public ServiceLocator(Service service, Service2 service2, Service3 service3)
        {
            _service = service;
            _service2 = service2;
            _service3 = service3;
        }

        public Hoge ResolveHoge()
        {
            return _resolveHogeCache ??= new Hoge(_service, _service2);
        }

        public Piyo ResolvePiyo()
        {
            return _resolvePiyoCache ??= new Piyo(ResolveHoge(), _service);
        }

        public Fuga ResolveFuga()
        {
            return _resolveFugaCache ??= new Fuga(_service, _service3);
        }
    }

}