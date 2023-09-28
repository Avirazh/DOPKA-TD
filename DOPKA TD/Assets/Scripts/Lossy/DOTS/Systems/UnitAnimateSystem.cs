using Lossy.DOTS.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
public partial struct UnitAnimateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        InstantiateUnitPrefab(ecb, ref state);

        PlayRunAnimation(ref state);

        TriggerDeathAnimation(ecb, SystemAPI.Time.DeltaTime, ref state);
        WaitForDeathAnimation(ecb, ref state);

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
                Value = instantiatedPrefab.GetComponent<Animator>()
            };

            entityCommandBuffer.AddComponent(entity, animatorReference);
        }
    }
    private void PlayRunAnimation(ref SystemState state)
    {
        foreach (var (transform, animatorReference) in
            SystemAPI.Query<LocalTransform, UnitAnimatorReferenceComponent>()
            .WithNone<DieAnimationTag>())
        {
            animatorReference.Value.SetBool("Run", true);
            animatorReference.Value.transform.position = transform.Position;
            animatorReference.Value.transform.rotation = transform.Rotation;
        }
    }
    private void TriggerDeathAnimation(EntityCommandBuffer entityCommandBuffer, float deltaTime, ref SystemState state)
    {

        foreach (var (transform, animator, entity) in
            SystemAPI.Query<LocalTransform, UnitAnimatorReferenceComponent>()
            .WithEntityAccess()
            .WithAny<DieAnimationTag>()
            .WithNone<DestroyTag, DieAnimationInProcessTag, TimerComponent>())
        {

            animator.Value.SetTrigger("Die");

            entityCommandBuffer.AddComponent<DieAnimationInProcessTag>(entity);

            //there should be config values
            if (animator.Value.GetBool("Die"))
                animator.Value.speed = 0.7f;
            animator.Value.transform.position = new Vector3(transform.Position.x, transform.Position.y - 2f * deltaTime, transform.Position.z);
            entityCommandBuffer.AddComponent(entity, new TimerComponent(2f));

        }
    }
    private void WaitForDeathAnimation(EntityCommandBuffer entityCommandBuffer, ref SystemState state)
    {
        foreach (var (dieAnimationInProcessTag, entity) in
            SystemAPI.Query<DieAnimationInProcessTag>()
            .WithEntityAccess()
            .WithNone<DestroyTag, TimerComponent>())
        {
            entityCommandBuffer.AddComponent<DestroyTag>(entity);
        }
    }
    private void RemoveUnitPrefab(EntityCommandBuffer entityCommandBuffer, ref SystemState state)
    {
        foreach (var (animatorReference, entity) in
           SystemAPI.Query<UnitAnimatorReferenceComponent>()
           .WithNone<UnitPrefabComponent, LocalTransform>()
           .WithEntityAccess())
        {
            Object.Destroy(animatorReference.Value.gameObject);
            entityCommandBuffer.RemoveComponent<UnitAnimatorReferenceComponent>(entity);
        }
    }
}
