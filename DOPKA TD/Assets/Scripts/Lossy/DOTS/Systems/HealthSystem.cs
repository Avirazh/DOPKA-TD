using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;

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
                EntityCommandBuffer = entityCommandBuffer,
            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
        public void Execute([ChunkIndexInQuery] int indexInQuery, HealthAspect health, HaveHitTag hitTag)
        {
            int damageToApply = 0;

            UnityEngine.Debug.Log("ApplyDamageJob was Executed");

            if(!health.DamageBuffer.IsEmpty)
            {
                foreach(var damage in health.DamageBuffer) 
                {
                    damageToApply += damage.Value; 
                }
                UnityEngine.Debug.Log(damageToApply + " dmg to " + "Entity => (" + health.Entity.Index + ", " + health.Entity.Version + ")");

                health.CurrentHealth -= damageToApply;

                health.DamageBuffer.Clear();
                EntityCommandBuffer.RemoveComponent<HaveHitTag>(indexInQuery, health.Entity);
            }

            if(health.CurrentHealth >= health.MaxHealth)
                health.CurrentHealth = health.MaxHealth;

            if (health.CurrentHealth <= 0)
                EntityCommandBuffer.AddComponent<DestroyTag>(indexInQuery, health.Entity);
        }
    }
}