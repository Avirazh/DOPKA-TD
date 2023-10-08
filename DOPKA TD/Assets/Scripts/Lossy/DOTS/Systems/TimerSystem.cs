using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    public partial struct TimerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) 
        {

        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new UpdateTimerJob
            {                
                DeltaTime = SystemAPI.Time.DeltaTime,

                EntityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),

            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct UpdateTimerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
        public float DeltaTime;

        public void Execute([EntityIndexInQuery] int indexInQuery, ref TimerComponent timer, Entity entity) 
        {           
            if(timer.IsDone)
            {
                EntityCommandBuffer.RemoveComponent<TimerComponent>(indexInQuery, entity);
            }
            timer.AddTime(DeltaTime);
        }
    }
}