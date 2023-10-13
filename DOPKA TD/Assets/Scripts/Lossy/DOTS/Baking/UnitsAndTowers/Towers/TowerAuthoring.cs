using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class TowerAuthoring : MonoBehaviour
    {
        public GameObject AttackZoneGameObject;
        public float AttackCooldown;
        public GameObject AttackProjectilePrefab;
        public Transform AttackSpawnPoint;
    }
    
    public class TowerBaker : Baker<TowerAuthoring>
    {
        public override void Bake(TowerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new TowerComponent()
            {
                AttackZoneEntity = GetEntity(authoring.AttackZoneGameObject, TransformUsageFlags.Dynamic),
                AttackCooldown = authoring.AttackCooldown,
                AttackProjectilePrefab = GetEntity(authoring.AttackProjectilePrefab, TransformUsageFlags.Dynamic),
                AttackSpawnPoint = authoring.AttackSpawnPoint.position
            });
        }
    }
}