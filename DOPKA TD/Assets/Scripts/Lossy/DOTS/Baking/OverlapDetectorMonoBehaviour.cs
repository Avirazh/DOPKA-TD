using System;
using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class OverlapDetectorMonoBehaviour : MonoBehaviour
    {
        public OverlapDetectorType OverlapDetectorType;
        public LayerMask LayerMask;
        public int ResultCount;
        public Color32 GizmoColor;
    }
    
    public class OverlapDetectorBaker : Baker<OverlapDetectorMonoBehaviour>
    {
        public override void Bake(OverlapDetectorMonoBehaviour authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            switch (authoring.OverlapDetectorType)
            {
                case OverlapDetectorType.Sphere:
                    AddComponent(entity, new SphereOverlapDetectorComponent() 
                    {
                        LayerMask = authoring.LayerMask,
                        ResultCount = authoring.ResultCount,
                        GizmoColor = authoring.GizmoColor
                    } );
                    break;
                case OverlapDetectorType.Box:
                    AddComponent(entity, new BoxOverlapDetectorComponent 
                    {
                        LayerMask = authoring.LayerMask,
                        ResultCount = authoring.ResultCount,
                        GizmoColor = authoring.GizmoColor
                    } );
                    break;
                case OverlapDetectorType.RayCast:
                    AddComponent(entity, new RaycastOverlapDetectorComponent 
                    {
                        LayerMask = authoring.LayerMask,
                        ResultCount = authoring.ResultCount,
                        GizmoColor = authoring.GizmoColor
                    } );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum OverlapDetectorType
    {
        Sphere,
        Box,
        RayCast
    }
}