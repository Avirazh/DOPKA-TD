using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Components
{
    public struct TowerComponent : IComponentData
    {
        public Entity AttackZoneEntity;
        public Entity TargetEntity;
        public float AttackCooldown;
        public float AttackTimer;
        public Entity AttackProjectilePrefab;
        public float3 AttackSpawnPoint;
    }
}