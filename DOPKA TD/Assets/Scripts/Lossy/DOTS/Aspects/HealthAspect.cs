using Lossy.DOTS.Components;
using Unity.Entities;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct HealthAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<MaxHealthComponent> _maxHealthComponent;
        private readonly RefRW<CurrentHealthComponent> _currentHealthComponent;
        private readonly DynamicBuffer<DamageBufferElement> _damageBuffer;

        public float CurrentHealth
        {
            get => _currentHealthComponent.ValueRO.Value;
            set => _currentHealthComponent.ValueRW.Value = value;
        } 
        public DynamicBuffer<DamageBufferElement> DamageBuffer => _damageBuffer;
    }
}