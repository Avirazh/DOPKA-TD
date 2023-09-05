using Lossy.Service;
using Zenject;

namespace Lossy.Installers
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle().NonLazy();
        }
    }
}