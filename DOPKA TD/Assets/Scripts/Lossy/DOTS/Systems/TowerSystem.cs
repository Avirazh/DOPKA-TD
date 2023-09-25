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
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
            
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
            
            state.Dependency.Complete();
            
            entityCommandBuffer.Playback(state.EntityManager);
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
                if (towerAspect.AttackAttackCooldown < towerAspect.AttackTimer && towerAspect.Target != default)
                {
                    var attackProjectile = EntityCommandBuffer.Instantiate(towerAspect.AttackProjectilePrefab);
                    EntityCommandBuffer.SetComponent(attackProjectile, new LocalTransform { Position = towerAspect.AttackSpawnPoint, Scale = 1f, Rotation = quaternion.identity });
                    EntityCommandBuffer.AddComponent(attackProjectile, new AttackProjectileTargetPositionComponent() { TargetPosition = LocalToWorldComponentLookup.GetRefRO(towerAspect.Target).ValueRO.Position });
                    towerAspect.ResetTimer();
                }
                towerAspect.AddTime(DeltaTime);
            }
        }
    }
}