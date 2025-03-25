using Source.Scripts.Drop;
using Source.Scripts.Inventory;
using Source.Scripts.CameraShaker;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Installers
{
    public class TowerLevelInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventorySettings _inventorySettings;
      
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TowerLevelInstaller>().FromInstance(this).AsSingle();
            Container.BindInterfacesTo<ResourceDataService>().AsSingle();
            
            
            
            Container.Bind<InventorySettings>().FromInstance(_inventorySettings);
            Container.Bind<CubeInventory>().AsSingle();
            Container.Bind<CubeMover>().AsSingle();
            Container.Bind<InventoryController>().AsSingle();
            Container.Bind<DropService>().AsSingle().NonLazy();
            Container.Bind<CameraShaker.CameraShaker>().AsSingle().NonLazy();

            Container.BindInterfacesTo<PCInput>().AsSingle();
         
            Container.BindInterfacesAndSelfTo<RayCaster>().AsSingle();
            Container.Bind<InventoryView>().FromInstance(_inventoryView);
        }

        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().Load();
        }
    }
}
