using System;
using System.Collections.Generic;
using Lossy.UI.Window;
using UnityEngine;

namespace Lossy.Factory
{
    public class WindowFactory
    {
        private const string Path = "UI/Windows";

        public event Action<DefaultWindow> WindowLoaded;

        private readonly GameFactory _gameFactory;
        private readonly Dictionary<string, DefaultWindow> _windows;
        
        private Canvas _windowCanvas;
        
        public WindowFactory(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            
            var windows = Resources.LoadAll<DefaultWindow>(Path);
            _windows = new Dictionary<string, DefaultWindow>();
            foreach (var window in windows)
                _windows.Add(window.name, window);
        }

        public void Init(Canvas windowCanvas)
        {
            _windowCanvas = windowCanvas;
        }

        public T GetWindow<T>() where T : DefaultWindow
        {
            DefaultWindow windowRealisation = _windows[typeof(T).Name];

            if (windowRealisation != null)
            {
                _windowCanvas.sortingOrder = _windowCanvas.sortingOrder;
                DefaultWindow window = _gameFactory.Instantiate<T>(windowRealisation.gameObject, _windowCanvas.transform);

                WindowLoaded?.Invoke(window);

                return (T)window;
            }

            Debug.LogWarning("Window not found: " + typeof(T));

            return default(T);
        }
    }
}