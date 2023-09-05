using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Lossy.Factory
{
    public class GameFactory
    {
        private DiContainer _diContainer;

        public GameFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void SetContainer(DiContainer diContainer) =>
            _diContainer = diContainer;

        public void Destroy(GameObject gameObject) =>
            Object.Destroy(gameObject);

        public GameObject Instantiate(GameObject prefab) =>
            _diContainer.InstantiatePrefab(prefab);

        public GameObject Instantiate(GameObject prefab, Transform parent) =>
            _diContainer.InstantiatePrefab(prefab, parent);

        public T Instantiate<T>(GameObject prefab) =>
            _diContainer.InstantiatePrefabForComponent<T>(prefab);

        public T Instantiate<T>(GameObject prefab, Transform parent) =>
            _diContainer.InstantiatePrefabForComponent<T>(prefab, parent);

        public T Instantiate<T>(Transform parent) where T : Component
        {
            T tmpComp = _diContainer.InstantiateComponentOnNewGameObject<T>();
            tmpComp.transform.SetParent(parent, false);
            return tmpComp;
        }

        public T Instantiate<T>(GameObject prefab, Transform parent, Vector3 position)
        {
            T element = _diContainer.InstantiatePrefabForComponent<T>(prefab, position, Quaternion.identity, parent);
            var monoBehaviourElement = element as MonoBehaviour;
            monoBehaviourElement.transform.localScale = Vector3.one;
            return element;
        }
    }
}