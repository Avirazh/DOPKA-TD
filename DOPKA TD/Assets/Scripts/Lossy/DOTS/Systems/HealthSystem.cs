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
                EntityCommandBufferParallelWriter = entityCommandBufferParallelWriter

            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferParallelWriter;

        public void Execute([ChunkIndexInQuery] int indexInQuery, HealthAspect health, HaveHitTag hitTag)
        {
            int damageToApply = 0;

            if (!health.DamageBuffer.IsEmpty)
            {
                foreach (var damage in health.DamageBuffer)
                {
                    damageToApply += damage.Value;
                }

                health.CurrentHealth -= damageToApply;

                health.DamageBuffer.Clear();
                EntityCommandBufferParallelWriter.RemoveComponent<HaveHitTag>(indexInQuery, health.Entity);
            }

            LimitCurrentHealth(health);
            CheckIfDead(indexInQuery, health, EntityCommandBufferParallelWriter );

        }

        private void LimitCurrentHealth(HealthAspect health)
        {
            if (health.CurrentHealth >= health.MaxHealth)
                health.CurrentHealth = health.MaxHealth;
        }

        private void CheckIfDead(int indexInQuery, HealthAspect health, EntityCommandBuffer.ParallelWriter entityCommandBufferParallelWriter)
        {
            if (health.CurrentHealth <= 0)
            {
                entityCommandBufferParallelWriter.AddComponent<DeathTag>(indexInQuery, health.Entity);
            }
        }

    }
}