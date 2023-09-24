using Unity.Entities;
using UnityEngine;

public class PortalAuthoring : MonoBehaviour
{
    public GameObject UnitDetectorGameObject;
    public class PortalBaker : Baker<PortalAuthoring>
    {
        public override void Bake(PortalAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PortalComponent 
            {
                UnitDetectorEntity = GetEntity(authoring.UnitDetectorGameObject, TransformUsageFlags.Dynamic) 
            });
        }
    }
}
