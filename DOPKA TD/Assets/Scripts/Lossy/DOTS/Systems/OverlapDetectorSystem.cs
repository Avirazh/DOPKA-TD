using Lossy.DOTS.Aspects;
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
            RaycastOverlap(state);
        }

        [BurstCompile]
        private void RaycastOverlap(SystemState state)
        {
            
            foreach (var raycastOverlapDetectorAspect in SystemAPI.Query<RaycastOverlapDetectorAspect>())
            {
                var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld.CollisionWorld;
                RaycastInput input = new RaycastInput()
                {
                    Start = raycastOverlapDetectorAspect.RayStartPosition,
                    End = (raycastOverlapDetectorAspect.RayStartPosition + (raycastOverlapDetectorAspect.RayDirection * raycastOverlapDetectorAspect.Distance)),
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = raycastOverlapDetectorAspect.CastLayerMask,
                        CollidesWith = raycastOverlapDetectorAspect.CastLayerMask, // all 1s, so all layers, collide with everything
                        GroupIndex = 0
                    }
                };
            
                Debug.DrawRay(
                    raycastOverlapDetectorAspect.RayStartPosition,
                    raycastOverlapDetectorAspect.RayDirection * raycastOverlapDetectorAspect.Distance,
                    raycastOverlapDetectorAspect.GizmoColor);
            
                RaycastHit hit = new RaycastHit();
                bool haveHit = collisionWorld.CastRay(input, out hit);
                if (haveHit)
                {
                    // see hit.Position
                    // // see hit.SurfaceNormal
                    Entity entity = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                    Debug.Log($"{raycastOverlapDetectorAspect.Entity.Index} RaycastHit {entity.Index} in {hit.Position} \n" +
                              $"Ray start at {raycastOverlapDetectorAspect.RayStartPosition} to {raycastOverlapDetectorAspect.RayDirection} with scale {raycastOverlapDetectorAspect.Distance} = {input.End}");
                }
            }
        }
    }
}