using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class TowerMonoBehaviour : MonoBehaviour
    {
        public GameObject AttackZoneGameObject;
        public float AttackCooldown;
        public GameObject AttackProjectilePrefab;
        public Transform AttackSpawnPoint;
    }
    
    public class TowerBaker : Baker<TowerMonoBehaviour>
    {
        public override void Bake(TowerMonoBehaviour authoring)
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