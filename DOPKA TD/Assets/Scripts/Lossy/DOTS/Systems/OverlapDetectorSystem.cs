using Lossy.DOTS.Aspects;
using Lossy.DOTS.Components;
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
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

            state.Dependency = new RemoveOverlapResultComponentJob()
            {
                EntityCommandBuffer = entityCommandBuffer
            }.Schedule(state.Dependency);
            
            state.Dependency = new SphereOverlapJob()
            {
                PhysicsWorldSingleton = physicsWorldSingleton,
                EntityCommandBuffer = entityCommandBuffer
            }.Schedule(state.Dependency);
            
            state.Dependency = new BoxOverlapJob()
            {
                PhysicsWorldSingleton = physicsWorldSingleton,
                EntityCommandBuffer = entityCommandBuffer
            }.Schedule(state.Dependency);
            
            state.Dependency = new RaycastOverlapJob
            {
                PhysicsWorldSingleton = physicsWorldSingleton,
                EntityCommandBuffer = entityCommandBuffer
            }.Schedule(state.Dependency);
            
            state.Dependency.Complete();
            
            entityCommandBuffer.Playback(state.EntityManager);
        }

        [BurstCompile]
        public partial struct SphereOverlapJob : IJobEntity
        {
            public PhysicsWorldSingleton PhysicsWorldSingleton;
            public EntityCommandBuffer EntityCommandBuffer;

            private void Execute(SphereOverlapDetectorAspect sphereOverlapDetectorAspect, DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements)
            {
                overlapResultBufferElements.Clear();

                var collisionWorld = PhysicsWorldSingleton.PhysicsWorld.CollisionWorld;
                var collisionFilter = new CollisionFilter()
                {
                    BelongsTo = sphereOverlapDetectorAspect.CastLayerMask,
                    CollidesWith =
                        sphereOverlapDetectorAspect.CastLayerMask,
                    GroupIndex = 0
                };
                
                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

                bool haveHit = collisionWorld.OverlapSphere(sphereOverlapDetectorAspect.StartPosition, sphereOverlapDetectorAspect.Radius, ref hits, collisionFilter);
                
                if (haveHit)
                {
                    for (var index = 0; index < hits.Length; index++)
                    {
                        if (index >= sphereOverlapDetectorAspect.ResultCount)
                            break;
                        
                        var hit = hits[index];
                        Entity entity = PhysicsWorldSingleton.CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                       
                        overlapResultBufferElements.Add(new OverlapResultBufferElement(){Entity = entity});
                    }

                    EntityCommandBuffer.AddComponent(sphereOverlapDetectorAspect.Entity, new OverlapResultTag());
                }
            }
        }

        [BurstCompile]
        public partial struct BoxOverlapJob : IJobEntity
        {
            public PhysicsWorldSingleton PhysicsWorldSingleton;
            public EntityCommandBuffer EntityCommandBuffer;

            private void Execute(BoxOverlapDetectorAspect boxOverlapDetectorAspect, DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements)
            {
                overlapResultBufferElements.Clear();

                var collisionWorld = PhysicsWorldSingleton.PhysicsWorld.CollisionWorld;
                var collisionFilter = new CollisionFilter()
                {
                    BelongsTo = boxOverlapDetectorAspect.CastLayerMask,
                    CollidesWith =
                        boxOverlapDetectorAspect.CastLayerMask,
                    GroupIndex = 0
                };
                
                NativeList<DistanceHit> hits = new NativeList<DistanceHit>(Allocator.Temp);

                bool haveHit = collisionWorld.OverlapBox(boxOverlapDetectorAspect.StartPosition, boxOverlapDetectorAspect.Rotation, boxOverlapDetectorAspect.Scale / 2, ref hits, collisionFilter);
                
                if (haveHit)
                {
                    for (var index = 0; index < hits.Length; index++)
                    {
                        if (index >= boxOverlapDetectorAspect.ResultCount)
                            break;
                        
                        var hit = hits[index];
                        Entity entity = PhysicsWorldSingleton.CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                       
                        overlapResultBufferElements.Add(new OverlapResultBufferElement(){Entity = entity});
                    }

                    EntityCommandBuffer.AddComponent(boxOverlapDetectorAspect.Entity, new OverlapResultTag());
                }
            }
        }
        
        [BurstCompile]
        public partial struct RaycastOverlapJob : IJobEntity
        {
            public PhysicsWorldSingleton PhysicsWorldSingleton;
            public EntityCommandBuffer EntityCommandBuffer;

            private void Execute(RaycastOverlapDetectorAspect raycastOverlapDetectorAspect, DynamicBuffer<OverlapResultBufferElement> overlapResultBufferElements)
            {
                overlapResultBufferElements.Clear();

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
                    Entity entity = PhysicsWorldSingleton.CollisionWorld.Bodies[hit.RigidBodyIndex].Entity;
                 
                    NativeArray<Entity> nativeEntities = new NativeArray<Entity>(1, Allocator.Temp);
                    nativeEntities[0] = entity;

                    EntityCommandBuffer.AddComponent(raycastOverlapDetectorAspect.Entity, new OverlapResultTag());
                    
                    overlapResultBufferElements.Add(new OverlapResultBufferElement() { Entity = entity });
                }
            }
        }

        private partial struct RemoveOverlapResultComponentJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;

            private void Execute(OverlapResultTag overlapResultTag, Entity entity)
            {
                EntityCommandBuffer.RemoveComponent(entity, typeof(OverlapResultTag));
            }
        }
    }
}