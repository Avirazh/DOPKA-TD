using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Components
{
    public struct TestCubeSpawnerComponent : IComponentData
    {
        public float3 SpawnPointPosition; 
        public Entity TestCube;
    }
}