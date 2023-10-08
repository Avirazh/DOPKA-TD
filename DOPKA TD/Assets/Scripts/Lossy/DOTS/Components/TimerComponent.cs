using Unity.Entities;

namespace Lossy.DOTS.Components
{
    public partial struct TimerComponent : IComponentData
    {
        public float ElapsedTime;
        public float TargetDuration;

        public TimerComponent(float targetDuration) : this()
        {
            SetDuration(targetDuration);
        }

        private void SetDuration(float targetDuration)
        {
            ElapsedTime = 0;
            TargetDuration = targetDuration;
        }

        public readonly bool IsDone
        {
            get { return ElapsedTime >= TargetDuration; }
        }

        public void AddTime(float time)
        {
            ElapsedTime += time;
        }
    }
}