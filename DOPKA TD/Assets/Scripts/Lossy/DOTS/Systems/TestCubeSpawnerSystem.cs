using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct TestCubeSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TestCubeSpawnerComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var testCubeSpawnerComponent = SystemAPI.GetSingletonEntity<TestCubeSpawnerComponent>();
            var testCubeSpawnerAspect = SystemAPI.GetAspect<TestCubeSpawnerAspect>(testCubeSpawnerComponent);

            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var spawnedTestCube = entityCommandBuffer.Instantiate(testCubeSpawnerAspect.TestCubePrefab);
            entityCommandBuffer.SetComponent(spawnedTestCube,new LocalTransform {Position = testCubeSpawnerAspect.SpawnPointPosition, Scale = 1f, Rotation = quaternion.identity});
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}