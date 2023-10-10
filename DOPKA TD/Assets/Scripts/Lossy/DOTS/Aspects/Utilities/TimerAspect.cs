using Lossy.DOTS.Components;
using Unity.Entities;
using Unity.Transforms;

namespace Lossy.DOTS.Aspects
{
    public readonly partial struct TimerAspect : IAspect
    {
        public readonly Entity entity;
        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRW<TimerComponent> _timerComponent;

        public float ElapsedTime => _timerComponent.ValueRW.ElapsedTime;
        public float TargetDuration => _timerComponent.ValueRO.TargetDuration;
        public readonly bool IsDone
        {
            get { return ElapsedTime >= TargetDuration; }
        }
        public void AddTime(float time)
        {
            _timerComponent.ValueRW.ElapsedTime += time;
        }
    }
}