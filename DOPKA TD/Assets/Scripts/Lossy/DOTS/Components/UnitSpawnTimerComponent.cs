using Unity.Entities;

namespace Lossy.DOTS.Components
{
    public partial struct UnitSpawnTimerComponent : IComponentData
    {
        public float TimeToNextSpawn;
        public float Timer;
    }
}