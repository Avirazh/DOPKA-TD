using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Lossy.DOTS.Components
{
    public struct BoxOverlapDetectorComponent : IComponentData
    {
        public int LayerMask;
        public int ResultCount;
        public float3 Scale;
        public Color32 GizmoColor;
    }
}