using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class RayCastOverlapDetectorAuthoring : MonoBehaviour
    {
        public LayerMask LayerMask;
        public float Distance;
        public Color32 GizmoColor;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            Debug.DrawRay(transform.position, transform.forward * Distance, GizmoColor);
        }
#endif
    }
    
    public class RayCastOverlapDetectorBaker : Baker<RayCastOverlapDetectorAuthoring>
    {
        public override void Bake(RayCastOverlapDetectorAuthoring authoring)
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