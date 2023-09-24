using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

public class PortalAuthoring : MonoBehaviour
{
    public GameObject UnitDetectorGameObject;
    public float Health;
    public class PortalBaker : Baker<PortalAuthoring>
    {
        public override void Bake(PortalAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PortalComponent 
            {
                UnitDetectorEntity = GetEntity(authoring.UnitDetectorGameObject, TransformUsageFlags.Dynamic) 
            });
            AddComponent(entity, new HealthComponent
            {
                Value = authoring.Health
            });
        }
    }
}
