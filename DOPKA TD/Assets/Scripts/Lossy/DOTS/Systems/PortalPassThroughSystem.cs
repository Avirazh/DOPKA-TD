using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Collections;

namespace Lossy.DOTS.Systems
{
    public partial struct PortalPassThroughSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DamageComponent>();
        }
        public void OnDestroy(ref SystemState state) 
        {

        }

        public void OnUpdate(ref SystemState state) 
        {
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            new PassThroughJob
            {
                DamageLookup = SystemAPI.GetComponentLookup<DamageComponent>(true),
                Ecb = ecb
            }.Schedule();

        }

        public partial struct PassThroughJob : IJobEntity
        {
            public EntityCommandBuffer Ecb;
            [ReadOnly] public ComponentLookup<DamageComponent> DamageLookup;
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