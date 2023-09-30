using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class UnitAnimatorAuthoring : MonoBehaviour
    {
        public GameObject UnitGameObjectPrefab;

        public class UnitGameObjectPrefabBaker : Baker<UnitAnimatorAuthoring>
        {
            public override void Bake(UnitAnimatorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponentObject(entity, new UnitPrefabComponent { Value = authoring.UnitGameObjectPrefab });
            }
        }
    }
}
