using System;

namespace Deprovgen.Try
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly Service _service;
        private readonly Service2 _service2;

        private Hoge? _resolveHogeCache;

        public ServiceLocator(Service service, Service2 service2)
        {
            _service = service;
            _service2 = service2;
        }

        public Hoge ResolveHoge()
        {
            return _resolveHogeCache ??= new Hoge(_service, _service2);
        }

    }

}