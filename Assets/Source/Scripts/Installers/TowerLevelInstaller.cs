using Source.Scripts.Inventory;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Installers
{
    public class TowerLevelInstaller : MonoInstaller
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventorySettings _inventorySettings;
      
        public override void InstallBindings()
        {
            Container.Bind<InventorySettings>().FromInstance(_inventorySettings);
            Container.Bind<CubeInventory>().AsSingle();
            Container.Bind<CubeHolder>().AsSingle();
            Container.Bind<InventoryController>().AsSingle();

            PCInput input = new PCInput();
            RayCaster rayCaster = new RayCaster(input);
            // Container.Bind<IInput>().To<MobileInput>().AsSingle();
            Container.Bind<IInput>().To<PCInput>().FromInstance(input).AsSingle();
            Container.Bind<RayCaster>().FromInstance(rayCaster).AsSingle();
            Container.Bind<InventoryView>().FromInstance(_inventoryView);
        }
    }
}
