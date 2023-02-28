using Interaction;
using Runner;
using System.Collections.Generic;
using UnityEngine;

namespace MzoUI.PlayScene
{
    public class PowerUpPanel : MonoBehaviour
    {
        public GameObject slotPrefab;
        public List<PowerUpSlot> slots;
        public void AddSlot(PowerUpCtrl newPowerUp)
        {
            GameObject newChild = Instantiate(slotPrefab, transform);
            PowerUpSlot slot = newChild.GetComponent<PowerUpSlot>();
            if(slot)
            {
                slots.Add(slot);
                // set start value
                slot.avatar.sprite = newPowerUp.data.avatar;
                slot.powerUpCtrl = newPowerUp;
                slot.slider.maxValue = newPowerUp.effectiveTime;
            }
        }
        public void RemoveSlot(PowerUpCtrl powerUp)
        {
            for(int i=0; i<transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if(child.GetComponent<PowerUpSlot>().powerUpCtrl.GetType() == powerUp.GetType())
                {
                    Destroy(child.gameObject);
                    return;
                }
            }
        }
    }
}
