using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct TestCubeSpawnerAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<TestCubeSpawnerComponent> _testCubeSpawnerComponent;

        public float3 SpawnPointPosition => _testCubeSpawnerComponent.ValueRO.SpawnPointPosition;
        public Entity TestCubePrefab => _testCubeSpawnerComponent.ValueRO.TestCube;
    }
}