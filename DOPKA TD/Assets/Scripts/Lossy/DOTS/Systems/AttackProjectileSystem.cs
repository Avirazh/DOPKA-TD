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
    [UpdateAfter(typeof(DestructionSystem))]
    public partial struct AttackProjectileSystem : ISystem
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
            
            state.Dependency = new MoveAttackProjectilesJob
            {
                EntityCommandBuffer = entityCommandBuffer,
                OverlapResultTagLookup = SystemAPI.GetComponentLookup<OverlapResultTag>(true),
                OverlapResultBufferLookup = SystemAPI.GetBufferLookup<OverlapResultBufferElement>(true),
                DeltaTime = SystemAPI.Time.DeltaTime
            }.Schedule(state.Dependency);

            new ExplodeProjectilesJob()
            {
                EntityCommandBuffer = entityCommandBuffer,
                OverlapResultTagLookup = SystemAPI.GetComponentLookup<OverlapResultTag>(true),
                OverlapResultBufferLookup = SystemAPI.GetBufferLookup<OverlapResultBufferElement>(true),
                DamageBufferLookup = SystemAPI.GetBufferLookup<DamageBufferElement>(false)
            }.Schedule();
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
            public BufferLookup<DamageBufferElement> DamageBufferLookup;
            
            private void Execute(ExplodeTag explodeTag, AttackProjectileAspect attackProjectileAspect)
            {
                UnityEngine.Debug.Log("ExplodeProjectilesJob executed");
                if (OverlapResultTagLookup.HasComponent(attackProjectileAspect.ExplodeZone))
                {
                    OverlapResultBufferLookup.TryGetBuffer(attackProjectileAspect.ExplodeZone, out var bufferData);
                    foreach (var overlapResultBufferElement in bufferData)
                    {
                        UnityEngine.Debug.Log("Attackprojectile overlapResult element" + overlapResultBufferElement.Entity.Index + ", " + overlapResultBufferElement.Entity.Version);

                        if (!DamageBufferLookup.HasBuffer(overlapResultBufferElement.Entity)) return;

                        DamageBufferLookup.TryGetBuffer(overlapResultBufferElement.Entity, out var damageBuffer);
                        damageBuffer.Add(new DamageBufferElement() { Value = attackProjectileAspect.DamageValue });

                        EntityCommandBuffer.AddComponent<HaveHitTag>(overlapResultBufferElement.Entity);
                    }
                }
                EntityCommandBuffer.AddComponent<DestroyTag>(attackProjectileAspect.Entity);
            }
        }
    }
}