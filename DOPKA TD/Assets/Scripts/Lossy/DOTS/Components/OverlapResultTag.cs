using Unity.Entities;

namespace Lossy.DOTS.Components
{
    public struct OverlapResultTag : IComponentData { }

    public struct OverlapResultBufferElement : IBufferElementData
    {
        public Entity Entity;
    }
}