using MailCore;
using Unity;
using Unity.Lifetime;

namespace MailRegWpf
{
	public static class ContainerHelper
	{
		public static IUnityContainer Container = new UnityContainer();

		static ContainerHelper()
		{
			Container.RegisterType<IEmailSupplier, EmailSupplierHttp>(new ContainerControlledLifetimeManager());
		}
	}
}