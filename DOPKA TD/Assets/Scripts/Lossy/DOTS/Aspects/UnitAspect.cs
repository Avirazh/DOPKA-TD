using ProjectDawn.Navigation;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Lossy.DOTS.Aspects
{
    public readonly partial struct UnitAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<MovableTag> _movableTag;
        private readonly RefRO<AgentBody> _agentBody;

        public void SetDestination(float3 destination)
        {
            _agentBody.ValueRO.SetDestination(destination);
        }
    }
}