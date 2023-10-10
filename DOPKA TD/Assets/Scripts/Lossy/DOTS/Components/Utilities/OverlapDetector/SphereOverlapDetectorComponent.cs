using Unity.Entities;
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
}