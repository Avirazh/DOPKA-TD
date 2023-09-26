using Lossy.DOTS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Lossy.DOTS.Systems
{
    [UpdateInGroup(typeof(PresentationSystemGroup),OrderFirst = true)]
    public partial struct UnitAnimateSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach(var (unitPrefabComponent, entity) in 
                    SystemAPI.Query<UnitPrefabComponent>()
                        .WithNone<UnitAnimatorReferenceComponent>()
                        .WithEntityAccess())
            {
                var instantiatedPrefab = Object.Instantiate(unitPrefabComponent.Value);
                var animatorReference = new UnitAnimatorReferenceComponent 
                {
                    Value = instantiatedPrefab.GetComponent<Animator>() 
                };

                ecb.AddComponent(entity, animatorReference);
            }

            foreach(var (transform, animatorReference) in 
                    SystemAPI.Query<LocalTransform, UnitAnimatorReferenceComponent>())
            {
                animatorReference.Value.SetBool("Run", true);
                animatorReference.Value.transform.position = transform.Position;
                animatorReference.Value.transform.rotation = transform.Rotation;
            }

            foreach( var (animatorReference, entity) in 
                    SystemAPI.Query<UnitAnimatorReferenceComponent>()
                        .WithNone<UnitPrefabComponent, LocalTransform>()
                        .WithEntityAccess())
            {
                Object.Destroy(animatorReference.Value.gameObject);
                ecb.RemoveComponent<UnitAnimatorReferenceComponent>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}
