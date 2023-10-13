using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using ProjectDawn.Navigation.Hybrid;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Lossy.DOTS.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TransformSystemGroup))]
    [UpdateBefore(typeof(ReadAgentTransformSystem))]
    [BurstCompile]
    public partial struct SpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) 
        {
            state.RequireForUpdate<SpawnerComponent>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var entityCommandBufferParallelWriter = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            new SpawnJob
            {
                DeltaTime = deltaTime,
                EntityCommandBufferParallelWriter = entityCommandBufferParallelWriter,
            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct SpawnJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferParallelWriter;

        [BurstCompile]
        public void Execute([ChunkIndexInQuery] int indexInQuery, SpawnerAspect spawner) 
        {
            spawner.TimeToNextSpawn -= DeltaTime;
            if(spawner.TimeToNextSpawn < 0 )
            {
                var newUnit = EntityCommandBufferParallelWriter.Instantiate(indexInQuery ,spawner.Prefab);

                spawner.TimeToNextSpawn = spawner.Timer;

                EntityCommandBufferParallelWriter.SetComponent(indexInQuery, newUnit, new LocalTransform { Position = spawner.SpawnPosition, Scale = 1f, Rotation = Quaternion.identity });
                EntityCommandBufferParallelWriter.AddComponent(indexInQuery, newUnit, new NewUnitTag { });
            }
        }
    }
}


