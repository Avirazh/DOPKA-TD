using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(DestructionSystem))]
    public partial struct GameStateSystem : ISystem
    {

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entityCommandBufferParallelWriter = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            new PortalDeathJob
            {
                EntityCommandBufferParallelWriter = entityCommandBufferParallelWriter,
            };
        }

        [BurstCompile]
        public partial struct PortalDeathJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter EntityCommandBufferParallelWriter;
            private void Execute([ChunkIndexInQuery] int indexInQuery, PortalAspect portal, DeathTag tag)
            {
                //there should be game over code
                EntityCommandBufferParallelWriter.AddComponent<DestroyTag>(indexInQuery, portal.Entity);
            }
        }
    }
}