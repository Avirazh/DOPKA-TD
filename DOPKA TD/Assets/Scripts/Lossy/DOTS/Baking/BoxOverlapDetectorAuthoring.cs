using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class BoxOverlapDetectorAuthoring : MonoBehaviour
    {
        public LayerMask LayerMask;
        public int ResultCount;
        public Vector3 Scale;
        public Color32 GizmoColor;
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireCube(transform.position, Scale);
        }
#endif
    }
    
    public class BoxCastOverlapDetectorBaker : Baker<BoxOverlapDetectorAuthoring>
    {
        public override void Bake(BoxOverlapDetectorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new BoxOverlapDetectorComponent()
            {
                LayerMask = authoring.LayerMask,
                ResultCount = authoring.ResultCount,
                GizmoColor = authoring.GizmoColor,
                Scale = authoring.Scale
            });
            
            AddBuffer<OverlapResultBufferElement>(entity);
        }
    }
}