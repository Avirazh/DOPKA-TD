using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class PortalAuthoring : MonoBehaviour
    {
        public class PortalBaker : Baker<PortalAuthoring>
        {
            public override void Bake(PortalAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PortalTag());
            }
        }
    }
}