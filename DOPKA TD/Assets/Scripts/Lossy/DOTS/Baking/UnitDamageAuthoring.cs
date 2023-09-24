using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class UnitDamageAuthoring : MonoBehaviour
    {
        public int DamageToPortalValue;
    }
    public class UnitDamageBaker : Baker<UnitDamageAuthoring> 
    {
        public override void Bake(UnitDamageAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new DamageComponent { Value = authoring.DamageToPortalValue });
        }
    }
}