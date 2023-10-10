using Unity.Entities;

namespace Lossy.DOTS.Components
{
    public struct OverlapResultBufferElement : IBufferElementData
    {
        public Entity Entity;
    }
}