using Assets.Scripts.Lossy.DOTS.Aspects;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateBefore(typeof(UnitAnimateSystem))]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct UnitMovementSystem : ISystem
    {
        private PortalAspect _portalAspect;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PortalTag>();
            state.RequireForUpdate<MovableTag>();
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
            state.Dependency = new UnitMovementJob { PortalPosition = _portalAspect.PortalPosition}.Schedule(state.Dependency);
        }

        public partial struct UnitMovementJob : IJobEntity
        {
            public float3 PortalPosition;
            void Execute(UnitAspect unitAspect)
            {
                Debug.Log("Unit with movementComponent is " + unitAspect.Entity.Index);
                Debug.Log("Target position " + PortalPosition);
                unitAspect.SetDestination(PortalPosition);
            }
        }
   
    }
}
