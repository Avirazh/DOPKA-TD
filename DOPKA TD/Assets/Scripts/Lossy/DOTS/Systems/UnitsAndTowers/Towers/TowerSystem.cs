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
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    
    public partial struct TowerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            state.Dependency = new FindTargetJob
            {
                OverlapResultTagLookup = SystemAPI.GetComponentLookup<OverlapResultTag>(true),
                OverlapResultBufferLookup = SystemAPI.GetBufferLookup<OverlapResultBufferElement>(true)
            }.Schedule(state.Dependency);
            
            state.Dependency = new AttackTargetJob
            {
                EntityCommandBuffer = entityCommandBuffer,
                DeltaTime = SystemAPI.Time.DeltaTime,
                LocalToWorldComponentLookup = SystemAPI.GetComponentLookup<LocalToWorld>(true)
            }.Schedule(state.Dependency);
        }
        
        [BurstCompile]
        public partial struct FindTargetJob : IJobEntity
        {
            [ReadOnly] public ComponentLookup<OverlapResultTag> OverlapResultTagLookup;
            [ReadOnly] public BufferLookup<OverlapResultBufferElement> OverlapResultBufferLookup;
            
            private void Execute(TowerAspect towerAspect)
            {
                towerAspect.ClearTarget();
                
                if (OverlapResultTagLookup.HasComponent(towerAspect.AttackZoneEntity))
                {
                    OverlapResultBufferLookup.TryGetBuffer(towerAspect.AttackZoneEntity, out var bufferData);
                    if (bufferData.Length > 0)
                        towerAspect.SetTarget(bufferData[0].Entity);
                }
            }
        }
        
        [BurstCompile]
        public partial struct AttackTargetJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;
            public float DeltaTime;
            [ReadOnly] public ComponentLookup<LocalToWorld> LocalToWorldComponentLookup;
            
            private void Execute(TowerAspect towerAspect)
            {
                if (towerAspect.AttackAttackCooldown < towerAspect.AttackTimer 
                    && LocalToWorldComponentLookup.TryGetComponent(towerAspect.Target, out var localTransform))
                {
                    var attackProjectile = EntityCommandBuffer.Instantiate(towerAspect.AttackProjectilePrefab);
                    EntityCommandBuffer.SetComponent(attackProjectile, new LocalTransform { Position = towerAspect.AttackSpawnPoint, Scale = 1f, Rotation = quaternion.identity });
                    EntityCommandBuffer.AddComponent(attackProjectile, new AttackProjectileTargetPositionComponent() { TargetPosition = localTransform.Position });

                    towerAspect.ResetTimer();
                }
                towerAspect.AddTime(DeltaTime);
            }
        }
    }
}