using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateBefore(typeof(UnitAnimateSystem))]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    [UpdateAfter(typeof(DestructionSystem))]
    public partial struct UnitMovementSystem : ISystem
    {
        private PortalAspect _portalAspect;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PortalTag>();
            state.RequireForUpdate<MovableTag>();
            state.RequireForUpdate<ProjectDawn.Navigation.AgentBody>();
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var portalEntity = SystemAPI.GetSingletonEntity<PortalTag>();
            _portalAspect = SystemAPI.GetAspect<PortalAspect>(portalEntity);
            state.Dependency = new UnitMovementJob { PortalPosition = _portalAspect.PortalPosition}.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public partial struct UnitMovementJob : IJobEntity
        {
            public float3 PortalPosition;
            
            void Execute(UnitAspect unitAspect)
            {
                unitAspect.SetDestination(PortalPosition);
            }
        }
   
    }
}
