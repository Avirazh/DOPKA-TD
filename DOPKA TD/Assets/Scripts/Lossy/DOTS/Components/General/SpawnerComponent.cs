using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Components
{
    public struct SpawnerComponent : IComponentData
    {
        public float3 SpawnPosition;
        public Entity Prefab;
    }
}