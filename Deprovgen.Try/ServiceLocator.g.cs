
namespace Deprovgen.Try
{
    class ServiceLocator : IServiceLocator
    {
        private readonly Service _service;

        private Hoge? _resolveHogeCache;

        public ServiceLocator(Service service)
        {
            _service = service;
        }

        public Hoge ResolveHoge()
        {
            return _resolveHogeCache ??= new Hoge(_service);
        }

    }
}