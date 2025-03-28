namespace Source.Scripts.Extensions
{
    public static class CollisionExtensions
    {
        public static int AsMask(this CollisionLayers layer)
        {
            return 1 << (int)layer;
        }
    }
}