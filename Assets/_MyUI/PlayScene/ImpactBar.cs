using UnityEngine;
using UnityEngine.UI;

namespace MzoUI.PlayScene
{
    public class ImpactBar : MonoBehaviour
    {
        public Slider slider;
        private void Start()
        {
            gameObject.SetActive(false);
        }
        public void Active(float maxValue)
        {
            gameObject.SetActive(true);
            slider.maxValue = maxValue;
        }
        public void Deactive()
        {
            gameObject.SetActive(false);
        }
    }
}
