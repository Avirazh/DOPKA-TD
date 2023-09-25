using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct AttackProjectileAspect : IAspect
    {
        public readonly Entity Entity;
        private readonly RefRO<AttackProjectileTargetPositionComponent> _attackProjectileTargetPositionComponent;
        
        private readonly RefRO<AttackProjectileComponent> _attackProjectileComponent;
        private readonly RefRO<LocalToWorld> _localToWorld;
        private readonly RefRW<LocalTransform> _localTransform;

        public Entity HitZone => _attackProjectileComponent.ValueRO.HitZone;
        public Entity ExplodeZone => _attackProjectileComponent.ValueRO.ExplodeZone;

        public float3 TargetPosition => _attackProjectileTargetPositionComponent.ValueRO.TargetPosition;
        public float3 Position => _localToWorld.ValueRO.Value.TransformPoint(float3.zero);
        public float Tolerance => _attackProjectileComponent.ValueRO.Tolerance;

        public void Move(float deltaTime)
        {
            _localTransform.ValueRW.Position += math.normalize(TargetPosition - _localTransform.ValueRO.Position) * deltaTime *
                                                _attackProjectileComponent.ValueRO.Speed;
        }
    }
}