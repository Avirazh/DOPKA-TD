using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    public partial struct ViewLayerReceiveSystem : ISystem
    {

        [BurstCompile]
        public void OnCreate(ref SystemState state) { }

        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var entityCommandBufferParallelWriter = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            new ApplyDestroyTagJob
            {
                EntityCommandBufferParallelWriter = entityCommandBufferParallelWriter,

            }.ScheduleParallel();
        }
    }
    [BurstCompile]
    public partial struct ApplyDestroyTagJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter EntityCommandBufferParallelWriter;
        public void Execute([ChunkIndexInQuery] int indexInQuery, Entity entity, DieAnimationEndedViewTag dieAnimationEndedViewTag)
        {
            EntityCommandBufferParallelWriter.AddComponent<DestroyTag>(indexInQuery, entity);
        }
    }
}