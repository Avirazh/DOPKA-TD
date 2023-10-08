using System;
using Unity.Entities;
using UnityEngine;

namespace Lossy.Animation
{
    public class AnimationEventCaster : MonoBehaviour
    {
        public event Action<EntityManager, Entity> DeadAnimationEnded;
        private void Start()
        {
            EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;    
        }

        public EntityManager EntityManager;
        public Entity Entity;
        public void OnDeadAnimationEnded()
        {
            DeadAnimationEnded?.Invoke(EntityManager, Entity);
            Debug.Log("invoked");
        }
    }
}
