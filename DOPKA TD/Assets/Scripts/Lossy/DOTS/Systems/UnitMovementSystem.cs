using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
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
            new UnitMovementJob { PortalPosition = _portalAspect.PortalPosition}.ScheduleParallel();
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
