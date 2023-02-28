using Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace MzoUI.PlayScene
{
    public class PowerUpSlot : MonoBehaviour
    {
        public PowerUpCtrl powerUpCtrl;
        public Slider slider;
        public Image avatar;

        private void Update()
        {
            if(powerUpCtrl != null)
            {
                slider.value = powerUpCtrl.effectiveTime;
            }
        }
    }
}
