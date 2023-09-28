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
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            new SpawnJob
            {
                DeltaTime = deltaTime,
                Ecb = ecb,
            }.Schedule();
        }
    }
    [BurstCompile]
    public partial struct SpawnJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer Ecb;

        [BurstCompile]
        public void Execute(SpawnerAspect spawner) 
        {
            spawner.TimeToNextSpawn -= DeltaTime;
            if(spawner.TimeToNextSpawn < 0 )
            {
                var newUnit = Ecb.Instantiate(spawner.Prefab);

                spawner.TimeToNextSpawn = spawner.Timer;

                Ecb.SetComponent(newUnit, new LocalTransform { Position = spawner.SpawnPosition, Scale = 1f, Rotation = Quaternion.identity });
                Ecb.AddComponent(newUnit, new NewUnitTag { });
            }
        }
    }
}


