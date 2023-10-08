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
            var entityCommandBufferParallelWriter = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            
            new ApplyDamageJob
            {
                UnitLookup = SystemAPI.GetComponentLookup<NewUnitTag>(true),
                EntityCommandBufferParallelWriter = entityCommandBufferParallelWriter

            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferParallelWriter;

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

                //UnityEngine.Debug.Log($"{damageToApply} dmg to Entity => ({health.Entity.Index}, {health.Entity.Version})"); // тоже самое, но выглядет аккуратнее

                health.CurrentHealth -= damageToApply;

                health.DamageBuffer.Clear();
                EntityCommandBufferParallelWriter.RemoveComponent<HaveHitTag>(indexInQuery, health.Entity);
            }
            
            if(health.CurrentHealth >= health.MaxHealth) 
                health.CurrentHealth = health.MaxHealth;

            if (health.CurrentHealth <= 0)
            {
                if (UnitLookup.HasComponent(health.Entity))
                {
                    EntityCommandBufferParallelWriter.AddComponent<DieAnimationTag>(indexInQuery, health.Entity);
                }
                else
                {
                    EntityCommandBufferParallelWriter.AddComponent<DestroyTag>(indexInQuery, health.Entity);
                }
            }

        }

    }
}