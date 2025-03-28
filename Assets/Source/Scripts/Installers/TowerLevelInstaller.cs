using Source.Scripts.ActionInfromer;
using Source.Scripts.Cameras;
using Source.Scripts.DragAndDrop;
using Source.Scripts.Inventory;
using Source.Scripts.ObjectPools;
using Source.Scripts.Save;
using Source.Scripts.Utils;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Installers
{
    public class TowerLevelInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private ActionInformerView _actionInformerView;
        [SerializeField] private InventorySettings _inventorySettings;
        [SerializeField] private CubeAnimationSettings _cubeAnimationSettings;
        [SerializeField] private Hole.Hole _hole;
        [SerializeField] private Tower _tower;
      
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TowerLevelInstaller>().FromInstance(this).AsSingle();
            Container.BindInterfacesTo<CameraProvider>().AsSingle().WithArguments(_camera);
            Container.BindInterfacesAndSelfTo<CubeObjectPoolProvider>().AsSingle().WithArguments(_cubeAnimationSettings.CubePrefab);
            Container.Bind<ActionInformerView>().FromInstance(_actionInformerView).AsSingle();
            Container.Bind<ActionInformerService>().AsSingle();
            Container.Bind<Hole.Hole>().FromInstance(_hole).AsSingle();
            Container.Bind<Tower>().FromInstance(_tower).AsSingle();
            Container.Bind<InventorySettings>().FromInstance(_inventorySettings).AsSingle();
            Container.Bind<CubeAnimationSettings>().FromInstance(_cubeAnimationSettings).AsSingle();
            Container.Bind<CubeInventory>().AsSingle();
            Container.BindInterfacesAndSelfTo<DragService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DragObjectMover>().AsSingle();
            Container.BindInterfacesAndSelfTo<DropService>().AsSingle().NonLazy();
            Container.Bind<CameraShaker>().AsSingle().NonLazy();
            Container.Bind<CubeAnimationService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<InventoryController>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<RayCaster>().AsSingle();
            Container.Bind<InventoryView>().FromInstance(_inventoryView);
        }

        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().Load();
            Container.Resolve<ISaveLoadService>().RegistryProgressSavers(_tower.gameObject);
            Container.Resolve<ISaveLoadService>().LoadProgress();
        }
    }
}
