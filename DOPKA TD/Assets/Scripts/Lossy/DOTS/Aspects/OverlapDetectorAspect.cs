using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Lossy.DOTS.Aspects
{
    //commented because it was throwing an exception 
    public readonly partial struct SphereOverlapDetectorAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalToWorld> _localToWorld;
        private readonly RefRO<SphereOverlapDetectorComponent> _sphereOverlapDetectorComponent;
        //private readonly DynamicBuffer<OverlapResultBufferElement> _overlapResultBufferElements;

        public float3 StartPosition => _localToWorld.ValueRO.Value.TransformPoint(float3.zero);
        public float Radius => _sphereOverlapDetectorComponent.ValueRO.Radius;
        public uint CastLayerMask => (uint) _sphereOverlapDetectorComponent.ValueRO.LayerMask;
        public int ResultCount => _sphereOverlapDetectorComponent.ValueRO.ResultCount;
        public Color32 GizmoColor => _sphereOverlapDetectorComponent.ValueRO.GizmoColor;
    }
    
    public readonly partial struct BoxOverlapDetectorAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalToWorld> _localToWorld;
        private readonly RefRO<BoxOverlapDetectorComponent> _boxOverlapDetectorComponent;
        //private readonly DynamicBuffer<OverlapResultBufferElement> _overlapResultBufferElements;
        
        public float3 StartPosition => _localToWorld.ValueRO.Value.TransformPoint(float3.zero);
        public quaternion Rotation => _localToWorld.ValueRO.Rotation;
        public uint CastLayerMask => (uint) _boxOverlapDetectorComponent.ValueRO.LayerMask;
        public Color32 GizmoColor => _boxOverlapDetectorComponent.ValueRO.GizmoColor;
        public int ResultCount => _boxOverlapDetectorComponent.ValueRO.ResultCount;
        public float3 Scale => _boxOverlapDetectorComponent.ValueRO.Scale;
    }
    
    public readonly partial struct RaycastOverlapDetectorAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRO<LocalToWorld> _localToWorld;
        private readonly RefRO<RaycastOverlapDetectorComponent> _raycastOverlapDetectorComponent;
        //private readonly DynamicBuffer<OverlapResultBufferElement> _overlapResultBufferElements;

        public float3 RayStartPosition => _localToWorld.ValueRO.Value.TransformPoint(float3.zero);
        public float3 RayDirection => _localToWorld.ValueRO.Value.Forward();
        public uint CastLayerMask => (uint) _raycastOverlapDetectorComponent.ValueRO.LayerMask;
        public Color32 GizmoColor => _raycastOverlapDetectorComponent.ValueRO.GizmoColor;
        public float Distance => _raycastOverlapDetectorComponent.ValueRO.Distance;
    }
}