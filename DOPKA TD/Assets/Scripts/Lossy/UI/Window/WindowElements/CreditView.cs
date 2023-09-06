using TMPro;
using UnityEngine;

namespace Lossy.UI.Window.WindowElements
{
    public class CreditView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private string _creditName;
        private string _jobTitle;

        public void Init(string creditName, string jobTitle)
        {
            _creditName = creditName;
            _jobTitle = jobTitle;
        }

        public void UpdateContent()
        {
            _text.text = $"{_creditName} - {_jobTitle}";
        }
    }
}