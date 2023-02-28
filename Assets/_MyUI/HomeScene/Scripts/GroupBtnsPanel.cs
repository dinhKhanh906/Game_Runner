using UnityEngine;

namespace MzoUI.HomeScene
{
    public class GroupBtnsPanel : MonoBehaviour
    {
        public void OnStart(string nameScenePlay)
        {
            // start game
            Loading loading = FindObjectOfType<Loading>();
            loading.OnLoadScene(nameScenePlay);
        }

        public void OnOpenInventory(GameObject inventoryPanel)
        {
            inventoryPanel.SetActive(true);
        }
        public void OnShowDetail(GameObject detailPanel)
        {
            // show detail
            detailPanel.SetActive(true);
        }
        public void OnHideDetail(GameObject detailPanel)
        {
            detailPanel.SetActive(false);
        }
        public void OnQuit()
        {
            Application.Quit();
        }
    }
}
