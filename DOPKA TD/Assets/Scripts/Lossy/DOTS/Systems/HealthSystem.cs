using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(DestructionSystem))]
    public partial struct HealthSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new ApplyDamageJob
            {
                UnitLookup = SystemAPI.GetComponentLookup<NewUnitTag>(true),
                EntityCommandBuffer = entityCommandBuffer
            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;

        [ReadOnly] public ComponentLookup<NewUnitTag> UnitLookup;
        public void Execute([ChunkIndexInQuery] int indexInQuery, HealthAspect health, HaveHitTag hitTag)
        {
            int damageToApply = 0;

            if(!health.DamageBuffer.IsEmpty)
            {
                foreach(var damage in health.DamageBuffer) 
                {
                    damageToApply += damage.Value; 
                }

                health.CurrentHealth -= damageToApply;

                health.DamageBuffer.Clear();
                EntityCommandBuffer.RemoveComponent<HaveHitTag>(indexInQuery, health.Entity);
            }

            if(health.CurrentHealth >= health.MaxHealth)
                health.CurrentHealth = health.MaxHealth;

            if (health.CurrentHealth <= 0)
                if (UnitLookup.HasComponent(health.Entity))
                {
                    EntityCommandBuffer.AddComponent<DieAnimationTag>(indexInQuery, health.Entity);
                }
                else
                {
                    EntityCommandBuffer.AddComponent<DestroyTag>(indexInQuery, health.Entity);
                }
            
        }

    }
}