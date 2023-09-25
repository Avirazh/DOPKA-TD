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
            public void Execute(PortalAspect portal, DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements)
            {
                if(overlapResultBufferElements.IsEmpty) return;

                foreach(var element in overlapResultBufferElements) 
                {
                    Ecb.AddComponent(element.Entity, new DestroyTag { });

                    int damageValue;

                    if (DamageLookup.TryGetComponent(element.Entity, out var damageComponent))
                        damageValue = damageComponent.Value;
                    else
                        damageValue = 0;

                    UnityEngine.Debug.Log(element.Entity.Index + ", " + element.Entity.Version);
  
                    portal.Health -= damageValue;

                }
            }
        }
    }
}