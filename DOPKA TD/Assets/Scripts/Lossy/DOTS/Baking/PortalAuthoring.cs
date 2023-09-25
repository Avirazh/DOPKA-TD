using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

public class PortalAuthoring : MonoBehaviour
{
    public float Health;
    public class PortalBaker : Baker<PortalAuthoring>
    {
        public override void Bake(PortalAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PortalComponent 
            {
            });
            AddComponent(entity, new HealthComponent
            {
                Value = authoring.Health
            });
        }
    }
}
