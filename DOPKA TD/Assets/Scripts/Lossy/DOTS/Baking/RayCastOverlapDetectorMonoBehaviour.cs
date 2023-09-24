using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class RayCastOverlapDetectorMonoBehaviour : MonoBehaviour
    {
        public LayerMask LayerMask;
        public float Distance;
        public Color32 GizmoColor;
    }
    
    public class RayCastOverlapDetectorBaker : Baker<RayCastOverlapDetectorMonoBehaviour>
    {
        public override void Bake(RayCastOverlapDetectorMonoBehaviour authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new RaycastOverlapDetectorComponent
            {
                LayerMask = authoring.LayerMask,
                GizmoColor = authoring.GizmoColor,
                Distance = authoring.Distance
            });

            AddBuffer<OverlapResultBufferElement>(entity);
        }
    }
}