using System.ComponentModel;
using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct OverlapDetectorSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new RaycastOverlapDetectJob{CollisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld.CollisionWorld}.ScheduleParallel();
             
        }

        private void RaycastOverlap()
        {
            
        }
    }

    [BurstCompile]
    public partial struct RaycastOverlapDetectJob : IJobEntity
    {
        public CollisionWorld CollisionWorld;
        
        [BurstCompile]
        private void Execute(RaycastOverlapDetectorAspect raycastOverlapDetectorAspect)
        {
            var collisionWorld = CollisionWorld;//.CollisionWorld;
            RaycastInput input = new RaycastInput()
            {
                Start = raycastOverlapDetectorAspect.RayStartPosition,
                End = raycastOverlapDetectorAspect.RayDirection,
                Filter = new CollisionFilter()
                {
                    BelongsTo = raycastOverlapDetectorAspect.CastLayerMask,
                    CollidesWith = 1u, // all 1s, so all layers, collide with everything
                    GroupIndex = 0
                }
            };
            
            Debug.DrawRay(
                raycastOverlapDetectorAspect.RayStartPosition,
                raycastOverlapDetectorAspect.RayDirection,
                raycastOverlapDetectorAspect.GizmoColor);
            
            RaycastHit hit = new RaycastHit();
            bool haveHit = collisionWorld.CastRay(input, out hit);
            if (haveHit)
            {
                // see hit.Position
                // // see hit.SurfaceNormal
                Entity entity = CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                Debug.Log($"RaycastHit {hit.Position}");
            }
            //return Entity.Null;
        }
    }
    
    [BurstCompile]
    public partial struct SphereOverlapDetectJob : IJobEntity
    {
        public PhysicsWorldSingleton PhysicsWorldSingleton;
        
        [BurstCompile]
        private void Execute(SphereOverlapDetectorComponent sphereOverlapDetectorComponent)
        {
        }
    }
    
    [BurstCompile]
    public partial struct BoxOverlapDetectJob : IJobEntity
    {
        public PhysicsWorldSingleton PhysicsWorldSingleton;
        
        [BurstCompile]
        private void Execute(BoxOverlapDetectorComponent boxOverlapDetectorComponent)
        {
            
        }
    }
}