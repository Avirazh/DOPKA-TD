using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(TowerSystem))]
    public partial struct AttackProjectileSystem : ISystem
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

            state.Dependency = new MoveAttackProjectilesJob
            {
                EntityCommandBuffer = entityCommandBuffer,
                OverlapResultTagLookup = SystemAPI.GetComponentLookup<OverlapResultTag>(true),
                OverlapResultBufferLookup = SystemAPI.GetBufferLookup<OverlapResultBufferElement>(true),
                DeltaTime = SystemAPI.Time.DeltaTime
            }.Schedule(state.Dependency);
            
            state.Dependency = new ExplodeProjectilesJob()
            {
                EntityCommandBuffer = entityCommandBuffer,
                OverlapResultTagLookup = SystemAPI.GetComponentLookup<OverlapResultTag>(true),
                OverlapResultBufferLookup = SystemAPI.GetBufferLookup<OverlapResultBufferElement>(true),
            }.Schedule(state.Dependency);
            
            state.Dependency.Complete();
            
            entityCommandBuffer.Playback(state.EntityManager);
        }
        
        [BurstCompile]
        public partial struct MoveAttackProjectilesJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;
            public float DeltaTime;
            [ReadOnly] public ComponentLookup<OverlapResultTag> OverlapResultTagLookup;
            [ReadOnly] public BufferLookup<OverlapResultBufferElement> OverlapResultBufferLookup;
            
            private void Execute(AttackProjectileAspect attackProjectileAspect)
            {
                attackProjectileAspect.Move(DeltaTime);
                if (OverlapResultTagLookup.HasComponent(attackProjectileAspect.HitZone))
                {
                    OverlapResultBufferLookup.TryGetBuffer(attackProjectileAspect.HitZone, out var bufferData);
                    if (bufferData.Length > 0 || math.distance(attackProjectileAspect.Position ,attackProjectileAspect.TargetPosition) < attackProjectileAspect.Tolerance)
                        EntityCommandBuffer.AddComponent(attackProjectileAspect.Entity, new ExplodeTag());
                }
            }
        }
        
        [BurstCompile]
        public partial struct ExplodeProjectilesJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;
            [ReadOnly] public ComponentLookup<OverlapResultTag> OverlapResultTagLookup;
            [ReadOnly] public BufferLookup<OverlapResultBufferElement> OverlapResultBufferLookup;
            
            private void Execute(ExplodeTag explodeTag, AttackProjectileAspect attackProjectileAspect)
            {
                if (OverlapResultTagLookup.HasComponent(attackProjectileAspect.ExplodeZone))
                {
                    OverlapResultBufferLookup.TryGetBuffer(attackProjectileAspect.ExplodeZone, out var bufferData);
                    foreach (var overlapResultBufferElement in bufferData)
                    {
                        //add component damage
                        //add component destroy
                    }
                }
            }
        }
    }
}