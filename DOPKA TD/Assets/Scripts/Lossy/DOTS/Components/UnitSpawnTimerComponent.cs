using Unity.Entities;

namespace Lossy.DOTS.Components
{
    public struct UnitSpawnTimerComponent : IComponentData
    {
        public float TimeToNextSpawn;
        public float Timer;
    }
}