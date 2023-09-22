using Lossy.DOTS.Aspects;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

namespace Lossy.DOTS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSystemGroup))]
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
            PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

            state.Dependency = new SphereOverlapJob()
            {
                PhysicsWorldSingleton = physicsWorldSingleton
            }.Schedule(state.Dependency);
            
            state.Dependency = new BoxOverlapJob()
            {
                PhysicsWorldSingleton = physicsWorldSingleton
            }.Schedule(state.Dependency);
            
            state.Dependency = new RaycastOverlapJob
            {
                PhysicsWorldSingleton = physicsWorldSingleton
            }.Schedule(state.Dependency);
        }

        [BurstCompile]
        public partial struct SphereOverlapJob : IJobEntity
        {
            public PhysicsWorldSingleton PhysicsWorldSingleton;

            public void Execute(SphereOverlapDetectorAspect sphereOverlapDetectorAspect)
            {
                var collisionWorld = PhysicsWorldSingleton.PhysicsWorld.CollisionWorld;
                var collisionFilter = new CollisionFilter()
                {
                    BelongsTo = sphereOverlapDetectorAspect.CastLayerMask,
                    CollidesWith =
                        sphereOverlapDetectorAspect.CastLayerMask, // all 1s, so all layers, collide with everything
                    GroupIndex = 0
                };
                
                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

                bool haveHit = collisionWorld.OverlapSphere(sphereOverlapDetectorAspect.StartPosition, sphereOverlapDetectorAspect.Radius, ref hits, collisionFilter);
                
                if (haveHit)
                {
                    foreach (var hit in hits)
                    {
                        Entity entity = PhysicsWorldSingleton.CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                        Debug.Log($"{sphereOverlapDetectorAspect.Entity.Index} SphereOverlap {entity.Index} in {hit.Position} \n" +
                                  $"SphereOverlap start at {sphereOverlapDetectorAspect.StartPosition} with radius {sphereOverlapDetectorAspect.Radius}");
                    }
                }
            }
        }

        [BurstCompile]
        public partial struct BoxOverlapJob : IJobEntity
        {
            public PhysicsWorldSingleton PhysicsWorldSingleton;

            public void Execute(BoxOverlapDetectorAspect boxOverlapDetectorAspect)
            {
                var collisionWorld = PhysicsWorldSingleton.PhysicsWorld.CollisionWorld;
                var collisionFilter = new CollisionFilter()
                {
                    BelongsTo = boxOverlapDetectorAspect.CastLayerMask,
                    CollidesWith =
                        boxOverlapDetectorAspect.CastLayerMask, // all 1s, so all layers, collide with everything
                    GroupIndex = 0
                };
                
                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

                bool haveHit = collisionWorld.OverlapBox(boxOverlapDetectorAspect.StartPosition, boxOverlapDetectorAspect.Rotation, boxOverlapDetectorAspect.Scale / 2, ref hits, collisionFilter);
                
                if (haveHit)
                {
                    foreach (var hit in hits)
                    {
                        Entity entity = PhysicsWorldSingleton.CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                        Debug.Log($"{boxOverlapDetectorAspect.Entity.Index} BoxOverlap {entity.Index} in {hit.Position} \n" +
                                  $"BoxOverlap start at {boxOverlapDetectorAspect.StartPosition} with radius {boxOverlapDetectorAspect.Scale}");
                    }
                }
            }
        }
        
        [BurstCompile]
        public partial struct RaycastOverlapJob : IJobEntity
        {
            public PhysicsWorldSingleton PhysicsWorldSingleton;

            public void Execute(RaycastOverlapDetectorAspect raycastOverlapDetectorAspect)
            {
                var collisionWorld = PhysicsWorldSingleton.PhysicsWorld.CollisionWorld;
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
                    Entity entity = PhysicsWorldSingleton.CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                    Debug.Log($"{raycastOverlapDetectorAspect.Entity.Index} RaycastHit {entity.Index} in {hit.Position} \n" +
                              $"Ray start at {raycastOverlapDetectorAspect.RayStartPosition} to {raycastOverlapDetectorAspect.RayDirection} with scale {raycastOverlapDetectorAspect.Distance} = {input.End}");
                }
            }
        }
    }
}