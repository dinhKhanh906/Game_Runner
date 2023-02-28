using UnityEngine;

namespace MzoUI.HomeScene
{
    public class ReportPanel : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }
        public void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}
