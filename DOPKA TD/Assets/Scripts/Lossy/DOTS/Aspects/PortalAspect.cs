using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct PortalAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<PortalComponent> _portalComponent;
        private readonly RefRO<LocalTransform> _localTransform;
        private readonly RefRW<HealthComponent> _healthComponent;
        public float3 PortalPosition => _localTransform.ValueRO.Position;
        public float Health
        {
            get => _healthComponent.ValueRO.Value;
            set => _healthComponent.ValueRW.Value = value;
        }
    }
}