using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;

namespace Lossy.DOTS.Systems
{

    [BurstCompile]
    [UpdateAfter(typeof(DestructionSystem))]
    public partial struct PortalPassThroughSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DamageComponent>();
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) 
        {

        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var entityCommandBufferParallelWriter = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new PassThroughJob
            {
                DamageLookup = SystemAPI.GetComponentLookup<DamageComponent>(true),
                EntityCommandBufferParallelWriter = entityCommandBufferParallelWriter
            }.ScheduleParallel();

        }
        [BurstCompile]
        public partial struct PassThroughJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter EntityCommandBufferParallelWriter;
            [ReadOnly] public ComponentLookup<DamageComponent> DamageLookup;

            public void Execute([ChunkIndexInQuery] int indexInQuery, PortalAspect portal, DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements, HealthAspect portalHealth)
            {
                if(overlapResultBufferElements.IsEmpty) return;

                foreach(var element in overlapResultBufferElements) 
                {
                    if (DamageLookup.HasComponent(element.Entity))
                    {
                        DamageLookup.TryGetComponent(element.Entity, out var damageComponent);

                        portalHealth.DamageBuffer.Add(new DamageBufferElement { Value = damageComponent.Value});

                        EntityCommandBufferParallelWriter.AddComponent(indexInQuery, portal.Entity, new HaveHitTag { });
                    }
                    EntityCommandBufferParallelWriter.AddComponent(indexInQuery, element.Entity, new DestroyTag { });
                }
            }
        }
    }
}