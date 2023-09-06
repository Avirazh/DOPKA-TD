using Lossy.Factory;
using Zenject;

namespace Lossy.Installers
{
    public class GameFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WindowFactory>().AsSingle().NonLazy();
        }
    }
}