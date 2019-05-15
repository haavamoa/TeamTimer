using LightInject;

namespace TeamTimer
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<MainPage>();
        }
    }
}