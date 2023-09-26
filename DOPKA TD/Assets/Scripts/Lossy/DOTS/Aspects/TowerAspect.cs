using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct TowerAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> _localTransform;
        private readonly RefRW<TowerComponent> _towerComponent;
        
        public Entity AttackZoneEntity => _towerComponent.ValueRO.AttackZoneEntity;
        public float AttackAttackCooldown => _towerComponent.ValueRO.AttackCooldown;
        public float AttackTimer => _towerComponent.ValueRO.AttackTimer;
        public Entity AttackProjectilePrefab => _towerComponent.ValueRO.AttackProjectilePrefab;
        public float3 AttackSpawnPoint => _towerComponent.ValueRO.AttackSpawnPoint;
        public Entity Target => _towerComponent.ValueRO.TargetEntity;

        public void SetTarget(Entity entity) =>
            _towerComponent.ValueRW.TargetEntity = entity;
        
        public void ClearTarget() => 
            _towerComponent.ValueRW.TargetEntity = default;

        public void AddTime(float time) =>
            _towerComponent.ValueRW.AttackTimer += time;

        public void ResetTimer() =>
            _towerComponent.ValueRW.AttackTimer = 0;
    }
}