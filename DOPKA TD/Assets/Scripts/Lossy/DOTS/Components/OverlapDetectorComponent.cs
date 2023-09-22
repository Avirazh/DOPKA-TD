using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Lossy.DOTS.Components
{ 
    public struct SphereOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public int ResultCount;
        public float Radius;
        public Color32 GizmoColor;
    }
    
    public struct BoxOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public int ResultCount;
        public float3 Scale;
        public Color32 GizmoColor;
    }
    
    public struct RaycastOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public float Distance;
        public Color32 GizmoColor;
    }
}