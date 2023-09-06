using Lossy.ScriptableObjects.Config;
using Lossy.UI.Window.WindowElements;
using UnityEngine;

namespace Lossy.UI.Window
{
    public class CreditsWindow : DefaultWindow
    {
        [SerializeField] private Transform _content;
        [SerializeField] private CreditView _creditViewPrefab;

        private CreditsConfigScriptableObject _creditsConfig;

        public void Init(CreditsConfigScriptableObject creditsConfig)
        {
            _creditsConfig = creditsConfig;
            base.Init();
        }

        public override void Show(bool animated = true)
        {
            InstantiateCreditViews();
            base.Show(animated);
        }

        private void InstantiateCreditViews()
        {
            foreach (var creditPair in _creditsConfig.CreditPairs)
            {
                CreditView creditView = Instantiate(_creditViewPrefab, _content); 
                creditView.Init(creditPair.CreditName, creditPair.JobTitle.ToString());
                creditView.UpdateContent();
            }
        }
    }
}