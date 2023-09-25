using Lossy.DOTS.Aspects;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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
            state.RequireForUpdate<PortalComponent>();
            state.RequireForUpdate<MovableTag>();
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var portalEntity = SystemAPI.GetSingletonEntity<PortalComponent>();
            _portalAspect = SystemAPI.GetAspect<PortalAspect>(portalEntity);
            state.Dependency = new UnitMovementJob { PortalPosition = _portalAspect.PortalPosition}.ScheduleParallel(state.Dependency);
        }

        [BurstCompile]
        public partial struct UnitMovementJob : IJobEntity
        {
            public float3 PortalPosition;
            void Execute(UnitAspect unitAspect)
            {
                //Debug.Log("Unit with movementComponent is " + unitAspect.Entity.Index);
                //Debug.Log("Target position " + PortalPosition);
                unitAspect.SetDestination(PortalPosition);
            }
        }
   
    }
}
