using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class SphereOverlapDetectorAuthoring : MonoBehaviour
    {
        public LayerMask LayerMask;
        public int ResultCount;
        public float Radius;
        public Color32 GizmoColor;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
#endif
    }
    
    public class SphereOverlapDetectorBaker : Baker<SphereOverlapDetectorAuthoring>
    {
        public override void Bake(SphereOverlapDetectorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new SphereOverlapDetectorComponent()
            {
                LayerMask = authoring.LayerMask,
                ResultCount = authoring.ResultCount,
                GizmoColor = authoring.GizmoColor,
                Radius = authoring.Radius
            });
            
            AddBuffer<OverlapResultBufferElement>(entity);
        }
    }
}