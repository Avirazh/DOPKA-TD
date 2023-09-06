using System;
using Lossy.Factory;
using Lossy.ScriptableObjects.Config;
using Lossy.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Lossy.UI.Page
{
    public class MainMenuPage : DefaultPage
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadLevelButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitButton;

        [Space] 
        
        [SerializeField] private CreditsConfigScriptableObject _creditsConfig;

        private WindowFactory _windowFactory;

        [Inject]
        public void Init(WindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
            
            _continueButton.onClick.AddListener(ContinueButtonOnClick);
            _newGameButton.onClick.AddListener(NewGameButtonOnClick);
            _loadLevelButton.onClick.AddListener(LoadLevelButtonOnClick);
            _creditsButton.onClick.AddListener(CreditsButtonOnClick);
            _quitButton.onClick.AddListener(QuitButtonOnClick);
        }

        public void Destruct()
        {
            _continueButton.onClick.RemoveListener(ContinueButtonOnClick);
            _newGameButton.onClick.RemoveListener(NewGameButtonOnClick);
            _loadLevelButton.onClick.RemoveListener(LoadLevelButtonOnClick);
            _creditsButton.onClick.RemoveListener(CreditsButtonOnClick);
            _quitButton.onClick.RemoveListener(QuitButtonOnClick);
        }

        private void ContinueButtonOnClick()
        {
            throw new NotImplementedException();
        }

        private void NewGameButtonOnClick()
        {
            NewGameInfoWindow newGameInfoWindow = _windowFactory.GetWindow<NewGameInfoWindow>();
            newGameInfoWindow.Init();
            newGameInfoWindow.Show();
        }

        private void LoadLevelButtonOnClick()
        {
            LoadLevelWindow loadLevelWindow = _windowFactory.GetWindow<LoadLevelWindow>();
            loadLevelWindow.Init();
            loadLevelWindow.Show();
        }

        private void CreditsButtonOnClick()
        {
            CreditsWindow creditsWindow = _windowFactory.GetWindow<CreditsWindow>();
            creditsWindow.Init(_creditsConfig);
            creditsWindow.Show();
        }

        private void QuitButtonOnClick() =>
            Application.Quit();
    }
}