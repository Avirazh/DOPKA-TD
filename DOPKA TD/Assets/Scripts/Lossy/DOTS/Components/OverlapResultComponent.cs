using Unity.Collections;
using Unity.Entities;

namespace Lossy.DOTS.Components
{
    public struct OverlapResultComponent : IComponentData
    {
        public NativeArray<Entity> Value;
    }
}