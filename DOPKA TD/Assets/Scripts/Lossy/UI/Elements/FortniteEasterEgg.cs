using UnityEngine;
using UnityEngine.UI;

namespace Lossy.UI.Elements
{
    public class FortniteEasterEgg : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Animator _animator;
        [SerializeField] private int _needTapCount;

        private int _currentTap = 0;
        private static readonly int Dance = Animator.StringToHash("Dance");

        private void OnEnable() =>
            _button.onClick.AddListener(ButtonOnClick);
    
        private void OnDisable() =>
            _button.onClick.RemoveListener(ButtonOnClick);
    

        private void ButtonOnClick()
        {
            _currentTap++;

            if (_currentTap == _needTapCount)
            {
                _animator.SetTrigger(Dance);
            }
        }
    }
}
