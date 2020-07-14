using System;

namespace Deprovgen.Try
{
    internal class ServiceLocatorFuga : IServiceLocatorFuga
    {
        private readonly Service _service;

        private Fuga? _resolveFugaCache;

        public ServiceLocatorFuga(Service service)
        {
            _service = service;
        }

        public Fuga ResolveFuga()
        {
            return _resolveFugaCache ??= new Fuga(_service);
        }

        public ServiceLocator ResolveServiceLocator(Service2 service2)
        {
            return new ServiceLocator(_service, service2);
        }
    }

    internal static class ServiceLocatorFugaExtensions
    {
        public static ServiceLocator ResolveServiceLocator(this IServiceLocatorFuga self, Service2 service2)
        {
            return self is ServiceLocatorFuga concrete ? concrete.ResolveServiceLocator(service2)
                : throw new NotImplementedException("このメソッドは ServiceLocatorFuga クラスに対してのみ呼び出せます。");
        }
    }
}