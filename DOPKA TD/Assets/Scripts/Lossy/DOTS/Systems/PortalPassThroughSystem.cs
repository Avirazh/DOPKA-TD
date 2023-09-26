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
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            new PassThroughJob
            {
                DamageLookup = SystemAPI.GetComponentLookup<DamageComponent>(true),
                Ecb = ecb
            }.Schedule();

        }
        [BurstCompile]
        public partial struct PassThroughJob : IJobEntity
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentLookup<DamageComponent> DamageLookup;

            [BurstCompile]
            public void Execute(PortalAspect portal, DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements, HealthAspect portalHealth)
            {
                if(overlapResultBufferElements.IsEmpty) return;

                foreach(var element in overlapResultBufferElements) 
                {
                    if (DamageLookup.HasComponent(element.Entity))
                    {
                        DamageLookup.TryGetComponent(element.Entity, out var damageComponent);

                        portalHealth.DamageBuffer.Add(new DamageBufferElement { Value = damageComponent.Value});

                        Ecb.AddComponent(portal.Entity, new HaveHitTag { });
                    }
                    Ecb.AddComponent(element.Entity, new DestroyTag { });
                }
            }
        }
    }
}