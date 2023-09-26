using Lossy.DOTS.Components;
using System.Collections;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class HealthAuthoring : MonoBehaviour
    {
        public int Health;
    }
    public class HealthBaker : Baker<HealthAuthoring> 
    {
        public override void Bake(HealthAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MaxHealthComponent { Value = authoring.Health });
            AddComponent(entity, new CurrentHealthComponent { Value = authoring.Health });
            AddBuffer<DamageBufferElement>(entity);
        }
    }
}