using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Components
{ 
    public struct SphereOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public int ResultCount;
        public Color32 GizmoColor;
    }
    
    public struct BoxOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public int ResultCount;
        public Color32 GizmoColor;
    }
    
    public struct RaycastOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public int ResultCount;
        public Color32 GizmoColor;
    }
}