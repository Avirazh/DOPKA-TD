using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Components
{
    public struct RaycastOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public float Distance;
        public Color32 GizmoColor;
    }
}