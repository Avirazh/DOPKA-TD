using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class AttackProjectileMonoBehaviour : MonoBehaviour
    {
        public int Damage;
        public float Speed;
        public float Tolerance;
        public GameObject HitPrefab;
        public GameObject HitZone;
        public GameObject ExplodeZone;
    }

    public class AttackProjectileBaker : Baker<AttackProjectileMonoBehaviour>
    {
        public override void Bake(AttackProjectileMonoBehaviour authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new AttackProjectileComponent()
            {
                Speed = authoring.Speed,
                HitPrefab = GetEntity(authoring.HitPrefab, TransformUsageFlags.Dynamic),
                HitZone = GetEntity(authoring.HitZone, TransformUsageFlags.Dynamic),
                ExplodeZone = GetEntity(authoring.ExplodeZone, TransformUsageFlags.Dynamic),
                Tolerance = authoring.Tolerance
            });
            AddComponent(entity, new DamageComponent()
            {
                Value = authoring.Damage,
            });
        }
    }
}