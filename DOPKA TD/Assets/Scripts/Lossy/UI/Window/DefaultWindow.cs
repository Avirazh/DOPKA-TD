using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lossy.UI.Window
{
    public class DefaultWindow : MonoBehaviour
    {
        public event Action<DefaultWindow> Showed;
        public event Action<DefaultWindow> Hided;

        [SerializeField] protected Animator _animator;
        [SerializeField] protected List<Button> _closeButtons;
        
        private bool _isActive;
        private bool _isStopped;
        private bool _isAnimation;
        private Action _actionOnClose;
        
        private readonly int _animationShowHash = Animator.StringToHash("Show");
        private readonly int _animationHideHash = Animator.StringToHash("Hide");

        public RectTransform RectTransform => (RectTransform) gameObject.transform;
        public bool IsActive => _isActive;
        public bool IsStopped => _isStopped;
        public bool IsAnimation => _isAnimation;

        protected virtual void Start()
        {
            foreach (var closeButton in _closeButtons)
                closeButton.onClick.AddListener(OnClickCloseButton);
        }

        public virtual void Init()
        {
            gameObject.SetActive(false);

            _isActive = false;
            _isStopped = false;
            _isAnimation = false;
        }

        protected void OnDestroy()
        {
            foreach (var closeButton in _closeButtons)
                closeButton.onClick.RemoveListener(OnClickCloseButton);
        }

        public virtual void Show(bool animation = true)
        {
            if (_isActive)
                return;

            gameObject.SetActive(true);

            _isActive = true;
            _isAnimation = true;

            Showed?.Invoke(this);
            Showed = null;

            if (_animator != null && animation)
            {
                _animator.enabled = true;
                _animator.Play(_animationShowHash);
                _animator.Update(0f);
            }
            else
            {
                OnEndShowAnimation();
            }
        }

        public virtual void Hide(bool animation = true)
        {
            _isStopped = false;
            _isAnimation = true;
            
            if (_animator != null && animation)
            {
                _animator.enabled = true;
                _animator.Play(_animationHideHash);
                _animator.Update(0f);
            }
            else
            {
                OnEndHideAnimation();
            }
        }

        public void Close() =>
            OnClickCloseButton();
        

        //

        protected virtual void OnEndShowAnimation()
        {
            _isStopped = true;
            _isAnimation = false;

            if (_animator != null)
                _animator.enabled = false;
        }

        protected virtual void OnEndHideAnimation()
        {
            gameObject.SetActive(false);

            _isAnimation = false;
            _isActive = false;

            if (_animator != null)
                _animator.enabled = false;

            Hided?.Invoke(this);
            Hided = null;
            
            Destroy(gameObject);
        }

        protected virtual void OnClickCloseButton()
        {
            Hide();
            if (_actionOnClose != null)
            {
                _actionOnClose.Invoke();
                _actionOnClose = null;
            }
        }

        public void SetActionOnClose(Action action)
        {
            _actionOnClose = action;
        }
    }
}
