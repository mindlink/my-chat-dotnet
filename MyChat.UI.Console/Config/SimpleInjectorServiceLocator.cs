using System;

namespace MindLink.Recruitment.MyChat.UI.Console.Config
{
    /// <summary>
    /// SimpleInjector adapter which implements the <see cref="IServiceLocator"/> interface.
    /// </summary>
    class SimpleInjectorServiceLocator: IServiceLocator
    {
        private SimpleInjector.Container _container;

        public SimpleInjectorServiceLocator(SimpleInjector.Container container)
        {
            if (container == null)
                throw new ArgumentNullException($"{nameof(container)}");

            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public T GetService<T>()
        {
            return (T) GetService(typeof(T));
        }
    }
}
