using UnityEngine;
using Runner;
using UnityEngine.UI;
using PlayerManager;

namespace MzoUI.PlayScene
{
    public class Menu : MonoBehaviour
    {
        public GameObject menuPanel;
        public Slider volumeSlider;
        private void Start()
        {
            menuPanel.SetActive(false);
            volumeSlider.onValueChanged.AddListener(UpdateVolume);
        }
        public void OnOpen()
        {
            menuPanel.SetActive(true);
            FindObjectOfType<CharacterInput>().hasListen = false;
            Time.timeScale = 0f;
        }
        public void OnClose()
        {
            menuPanel.SetActive(false);
            FindObjectOfType<CharacterInput>().hasListen = true;
            Time.timeScale = 1f;
        }
        public void OnLoadHomeScene()
        {
            Time.timeScale = 1f;
            Loading loading = FindObjectOfType<Loading>();
            if(loading) loading.OnLoadScene("HomeScene");
        }
        private void UpdateVolume(float value)
        {
            Audio.instance.source.volume = value;
            DataSystem.SetVolume(value);
            Debug.Log(value);
        }
    }
}
