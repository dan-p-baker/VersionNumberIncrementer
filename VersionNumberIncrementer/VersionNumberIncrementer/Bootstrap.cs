using SimpleInjector;
using VersionNumberIncrementer.Domain;

namespace VersionNumberIncrementer
{
    internal class Bootstrap
    {
        public static Container Container;

        public static void Start()
        {
            Container = new Container();

            Container.Register<IReleaseService, ReleaseService>(Lifestyle.Singleton); 
        }
    }
}