using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using PlayerManager;

namespace MzoUI
{
    public class Loading : MonoBehaviour
    {
        public GameObject loadingPanel;
        public Slider slider;
        private void Start()
        {
            loadingPanel.gameObject.SetActive(false);
        }
        public void OnLoadScene(string nameScene)
        {
            // load scene
            loadingPanel.gameObject.SetActive(true);
            // set character in player scene
            if(nameScene == "PlayScene") GameManager.instance.SetUpForPlayScene();
            StartCoroutine(OnLoadingAsync(nameScene));
        }
        IEnumerator OnLoadingAsync(string nameScene)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(nameScene);
            while (!operation.isDone)
            {
                slider.value = operation.progress;
                yield return null;
            }
        }
    }
}
