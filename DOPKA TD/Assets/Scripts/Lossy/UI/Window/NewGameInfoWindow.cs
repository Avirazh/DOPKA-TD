using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lossy.UI.Window
{
    public class NewGameInfoWindow : DefaultWindow
    {
        public event Action Applied;

        [SerializeField] private Button _applyButton;

        public override void Init()
        {
            _applyButton.onClick.AddListener(ApplyButtonOnClick);
            base.Init();
        }

        public override void Hide(bool animation = true)
        {
            _applyButton.onClick.RemoveListener(ApplyButtonOnClick);
            base.Hide(animation);
        }

        private void ApplyButtonOnClick() =>
            Applied?.Invoke();
        
    }
}