using Assets.Scripts.Lossy.DOTS.Aspects;
using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using ProjectDawn.Navigation;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(AgentSteeringSystemGroup))]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct UnitMovementSystem : ISystem
{
    private PortalAspect _portalAspect;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PortalTag>();
        state.RequireForUpdate<MovableTag>();


        var portalEntity = SystemAPI.GetSingletonEntity<PortalTag>();
        _portalAspect = SystemAPI.GetAspect<PortalAspect>(portalEntity);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new UnitMovementJob { PortalPosition = _portalAspect.PortalPosition}.Schedule(state.Dependency);
    }
    public partial struct UnitMovementJob : IJobEntity
    {
        public float3 PortalPosition;
        void Execute(UnitAspect unitAspect)
        {
            unitAspect.SetDestination(PortalPosition);
        }
    }
   
}
