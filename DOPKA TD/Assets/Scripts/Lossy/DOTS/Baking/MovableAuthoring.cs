using Lossy.DOTS.Components;
using Unity.Entities;
using UnityEngine;

namespace Lossy.DOTS.Baking
{
    public class MovableAuthoring : MonoBehaviour
    {
        public class MovableBaker : Baker<MovableAuthoring> 
        {
            public override void Bake(MovableAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new MovableTag { });
            }
        }
    }
}
