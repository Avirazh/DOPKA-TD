using System.Collections;
using UnityEngine.SceneManagement;

namespace Lossy.Service
{
    public class SceneService
    {
        public IEnumerator LoadScene(int sceneId)
        {
            yield return SceneManager.LoadSceneAsync(sceneId);
        }

        public IEnumerator LoadMenuScene()
        {
            yield return LoadScene(1);
        }
    }
}