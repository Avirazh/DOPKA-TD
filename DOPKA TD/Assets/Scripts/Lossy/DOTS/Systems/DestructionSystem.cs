using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    public partial struct DestructionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) 
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }
        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var entityCommandBuffer = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            new DestructionJob
            {
                EntityCommandBuffer = entityCommandBuffer

            }.Schedule();

            //state.Dependency.Complete();
            //entityCommandBuffer.Playback(state.EntityManager);
        }
        [BurstCompile]
        public partial struct DestructionJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;

            [BurstCompile]
            public void Execute(in DestroyTag destroyTag, Entity entity)
            {
                //EntityCommandBuffer.RemoveComponent<UnitMovementSystem>(entity);
                EntityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}