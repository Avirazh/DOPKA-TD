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
            UnityEngine.Debug.Log("TimerComponent SetDuration");
            ElapsedTime = 0;
            TargetDuration = targetDuration;
        }
    }
}