using System.Collections;
using Lossy.Factory;
using Lossy.Service;
using UnityEngine;
using Zenject;

namespace Lossy.Bootstrapper
{
    public class MainBootstrapper : MonoBehaviour
    {
        [SerializeField] private Canvas _windowCanvas;
        
        private WindowFactory _windowFactory;
        private SceneService _sceneService;
        
        [Inject]
        public void Construct(WindowFactory windowFactory, SceneService sceneService)
        {
            _windowFactory = windowFactory;
            _sceneService = sceneService;
        }

        public void Start()
        {
            StartCoroutine(InitCoroutine());
        }

        public IEnumerator InitCoroutine()
        {
            DontDestroyOnLoad(_windowCanvas);
            
            _windowFactory.Init(_windowCanvas);

            yield return _sceneService.LoadMenuScene();
        }
    }
}