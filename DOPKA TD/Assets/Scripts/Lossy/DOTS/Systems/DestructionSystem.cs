using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    public partial struct DestructionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var entityCommandBuffer = new EntityCommandBuffer();

            new DestructionJob
            {
                EntityCommandBuffer = entityCommandBuffer

            }.Schedule(); 
        }
        [BurstCompile]
        public partial struct DestructionJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;

            [BurstCompile]
            public void Execute(DestroyTag destroyTag, Entity entity)
            {
                EntityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}