using System;
using System.Reflection;
using LightInject;

namespace TeamTimer.Tests
{
    public class TestBase
    {
        public TestBase()
        {
            var container = CreateContainer();
            Configure(container);
            container.RegisterFrom<CompositionRoot>();
            ServiceFactory = container.BeginScope();
            InjectPrivateFields();
        }

        private void InjectPrivateFields()
        {
            var privateInstanceFields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var privateInstanceField in privateInstanceFields)
            {
                privateInstanceField.SetValue(this, GetInstance(privateInstanceField));
            }
        }

        internal Scope ServiceFactory { get; }

        public void Dispose() => ServiceFactory.Dispose();

        public TService GetInstance<TService>(string name = "")
            => ServiceFactory.GetInstance<TService>(name);

        private object GetInstance(FieldInfo field)
        {
            return ServiceFactory.TryGetInstance(field.FieldType) ?? ServiceFactory.TryGetInstance(field.FieldType, field.Name);
        } 

        internal virtual IServiceContainer CreateContainer() => new ServiceContainer();

        internal virtual void Configure(IServiceRegistry serviceRegistry) { }
    }
}