using Lossy.DOTS.Components;
using Lossy.DOTS.Systems;
using Unity.Entities;
using UnityEngine;

namespace Lossy.Animation
{
    public class UnitAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private Entity _entity;
        private EntityManager _entityManager;

        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Run = Animator.StringToHash("Run");

        private void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;    
        }

        public void SetEntity(Entity entity) => 
            _entity = entity;

        public void PlayDieAnimation() =>
            _animator.SetTrigger(Die);

        public void SetRunBool(bool run) => 
            _animator.SetBool(Run, run);

        public void OnDieAnimationEnded()
        {
            _entityManager.AddComponent<DieAnimationEndedViewTag>(_entity);
        }
    }
}
