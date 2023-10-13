using Unity.Entities;
using Unity.Mathematics;

namespace Lossy.DOTS.Components
{
    public struct AttackProjectileComponent : IComponentData
    {
        public float Speed;
        public float Tolerance;
        public Entity HitPrefab;

        public Entity HitZone;
        public Entity ExplodeZone;
    }

    public struct AttackProjectileTargetPositionComponent : IComponentData
    {
        public float3 TargetPosition;
    }

    public struct ExplodeTag : IComponentData
    {
    }
}