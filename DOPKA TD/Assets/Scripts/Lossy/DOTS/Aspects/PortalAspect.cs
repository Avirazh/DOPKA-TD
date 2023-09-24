using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts.Lossy.DOTS.Aspects
{
    public readonly partial struct PortalAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<PortalTag> _portalTag;
        private readonly RefRO<LocalTransform> _localTransform;
        public float3 PortalPosition => _localTransform.ValueRO.Position;
    }
}