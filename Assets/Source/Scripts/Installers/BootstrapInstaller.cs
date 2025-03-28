using Source.Scripts.Inventory;
using Source.Scripts.Localization;
using Source.Scripts.Save;
using Zenject;

namespace Source.Scripts.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResourceDataService>().AsSingle();
            Container.Bind<ISaveLoadService>().To<PlayerPrefsSaveLoadService>().AsSingle();
            Container.Bind<LocalizationService>().AsSingle();
            
            Container.BindInterfacesTo<PCInput>().AsSingle();
        }
    }
}