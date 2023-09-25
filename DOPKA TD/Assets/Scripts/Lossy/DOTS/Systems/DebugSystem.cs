using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Systems;
using UnityEngine;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(OverlapDetectorSystem))]
    [UpdateInGroup(typeof(PhysicsSimulationGroup))]
    public partial struct DebugSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Dependency = new DebugJob().Schedule(state.Dependency);
        }

        [BurstCompile]
        public partial struct DebugJob : IJobEntity
        {
            private void Execute(DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements, Entity entity)
            {
                if (overlapResultBufferElements.Length > 0)
                    Debug.Log($"entity {entity.Index} contains {overlapResultBufferElements[0].Entity.Index}");
            }
        }
    }
}