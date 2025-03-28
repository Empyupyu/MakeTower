using Source.Scripts.Inventory;

namespace Source.Scripts.ObjectPools
{
    public class CubeObjectPoolProvider
    {
        public ObjectPool<Cube> ObjectPool { get; private set; }

        public CubeObjectPoolProvider(Cube prefab)
        {
            ObjectPool = new ObjectPool<Cube>(prefab);
        }
    }
}