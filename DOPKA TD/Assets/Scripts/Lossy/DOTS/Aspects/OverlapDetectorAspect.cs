using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct SphereOverlapDetectorAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> _localTransform;
        private readonly RefRO<SphereOverlapDetectorComponent> _sphereOverlapDetectorComponent;
        
        public float3 StartPosition => _localTransform.ValueRO.Position;
        public float Radius => _sphereOverlapDetectorComponent.ValueRO.Radius;
        public uint CastLayerMask => (uint) _sphereOverlapDetectorComponent.ValueRO.LayerMask;
        public Color32 GizmoColor => _sphereOverlapDetectorComponent.ValueRO.GizmoColor;
    }
    
    public readonly partial struct BoxOverlapDetectorAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalTransform> _localTransform;
        private readonly RefRO<BoxOverlapDetectorComponent> _boxOverlapDetectorComponent;
        
        public float3 StartPosition => _localTransform.ValueRO.Position;
        public quaternion Rotation => _localTransform.ValueRO.Rotation;
        public uint CastLayerMask => (uint) _boxOverlapDetectorComponent.ValueRO.LayerMask;
        public Color32 GizmoColor => _boxOverlapDetectorComponent.ValueRO.GizmoColor;
        public float3 Scale => _localTransform.ValueRO.Scale;
    }
    
    public readonly partial struct RaycastOverlapDetectorAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalTransform> _localTransform;
        private readonly RefRO<RaycastOverlapDetectorComponent> _raycastOverlapDetectorComponent;

        public float3 RayStartPosition => _localTransform.ValueRO.Position;
        public float3 RayDirection => _localTransform.ValueRO.Forward();
        public uint CastLayerMask => (uint) _raycastOverlapDetectorComponent.ValueRO.LayerMask;
        public Color32 GizmoColor => _raycastOverlapDetectorComponent.ValueRO.GizmoColor;
        public float Distance => _raycastOverlapDetectorComponent.ValueRO.Distance;
    }
}