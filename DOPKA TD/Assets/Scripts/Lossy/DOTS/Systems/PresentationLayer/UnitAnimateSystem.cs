using Lossy.Animation;
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

            InstantiateUnitPrefab(ecb, ref state);

            PlayRunAnimation(ecb, ref state);
            MoveGameObject(ref state);

            TriggerDeathAnimation(ecb, ref state);

            RemoveUnitPrefab(ecb, ref state);

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        private void InstantiateUnitPrefab(EntityCommandBuffer entityCommandBuffer, ref SystemState state)
        {
            foreach (var (unitPrefabComponent, entity) in
                SystemAPI.Query<UnitPrefabComponent>()
                .WithNone<UnitAnimatorReferenceComponent>()
                .WithEntityAccess())
            {
                var instantiatedPrefab = Object.Instantiate(unitPrefabComponent.Value);
                var animatorReference = new UnitAnimatorReferenceComponent
                {
                    AnimatorController = instantiatedPrefab.GetComponent<UnitAnimatorController>()
                };

                animatorReference.AnimatorController.SetEntity(entity);
                entityCommandBuffer.AddComponent(entity, animatorReference);
                
            }
        }

        private void PlayRunAnimation(EntityCommandBuffer entityCommandBuffer, ref SystemState state)
        {
            foreach (var (animator, entity) in
                SystemAPI.Query<UnitAnimatorReferenceComponent>()
                .WithEntityAccess()
                .WithNone<DeathTag, MovableTag>())
            {
                animator.AnimatorController.SetRunBool(true);

                entityCommandBuffer.AddComponent<MovableTag>(entity);
            }
        }

        private void MoveGameObject(ref SystemState state)
        {
            foreach (var (transform, animator) in
                SystemAPI.Query<LocalTransform, UnitAnimatorReferenceComponent>()
                .WithAny<MovableTag>()
                .WithNone<DeathTag>())
            {
                animator.AnimatorController.transform.SetPositionAndRotation(transform.Position, transform.Rotation);           
            }
        }

        private void TriggerDeathAnimation(EntityCommandBuffer entityCommandBuffer, ref SystemState state)
        {
            foreach (var (animator, entity) in
                SystemAPI.Query<UnitAnimatorReferenceComponent>()
                .WithEntityAccess()
                .WithAll<DeathTag, MovableTag>()
                .WithNone<DestroyTag>())
            {
                animator.AnimatorController.PlayDieAnimation();

                entityCommandBuffer.RemoveComponent<MovableTag>(entity);
            }
        }        

        private void RemoveUnitPrefab(EntityCommandBuffer entityCommandBuffer, ref SystemState state)
        {
            foreach (var (animator, entity) in
               SystemAPI.Query<UnitAnimatorReferenceComponent>()
               .WithNone<UnitPrefabComponent, LocalTransform>()
               .WithEntityAccess())
            {
                Object.Destroy(animator.AnimatorController.gameObject);
                entityCommandBuffer.RemoveComponent<UnitAnimatorReferenceComponent>(entity);
            }
        }
    }
}
