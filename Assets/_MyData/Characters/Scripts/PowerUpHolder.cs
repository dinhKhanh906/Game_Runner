using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Interaction;
using MzoUI.PlayScene;

namespace Runner
{
    public class PowerUpHolder : MonoBehaviour
    {
        public PowerUpPanel panel;
        public TextNotice textNotice;
        public List<PowerUpCtrl> powerUps = new List<PowerUpCtrl>();
        Queue<PowerUpCtrl> indexsNeedRemove = new Queue<PowerUpCtrl>();
        private void Awake()
        {
            if(panel==null) panel = FindObjectOfType<PowerUpPanel>();
            if(textNotice==null) textNotice = FindObjectOfType<TextNotice>();
        }
        private void Start()
        {
            textNotice.gameObject.SetActive(false);
        }
        private void Update()
        {
            for(int i=0; i<powerUps.Count; i++)
            {
                // timer
                powerUps[i].effectiveTime -= Time.deltaTime;
                if (powerUps[i].effectiveTime <= 0f)
                {
                    if(!powerUps[i].wasReturn) powerUps[i].ReturnForBeginning();
                    indexsNeedRemove.Enqueue(powerUps[i]);
                }
            }
            // remove power ups was ended
            for(int i=0; i<indexsNeedRemove.Count; i++)
            {
                PowerUpCtrl needRemove = indexsNeedRemove.Dequeue();
                RemovePowerUp(needRemove);
                Destroy(needRemove.gameObject);
            }
        }
        public void AddNewPowerUp(PowerUpCtrl newPowerUp)
        {
            powerUps.Add(newPowerUp);
            panel.AddSlot(newPowerUp);
            newPowerUp.transform.parent = transform;
        }
        public void RemovePowerUp(PowerUpCtrl powerUp)
        {
            powerUps.Remove(powerUp); 
            panel.RemoveSlot(powerUp);
        }
        public void Insert(PowerUpCtrl newPowerUp)
        {
            textNotice.OnActive(newPowerUp.data.name + "!!");
            if (powerUps.Count > 0)
            {
                for (int i = 0; i < powerUps.Count; i++)
                {
                    // check this new power up has or not yet
                    if (powerUps[i].GetType() == newPowerUp.GetType())
                    {
                        powerUps[i].effectiveTime = newPowerUp.effectiveTime;
                        return;
                    }
                    else if (i == powerUps.Count - 1 && powerUps[i].GetType() != newPowerUp.GetType())
                    {
                        AddNewPowerUp(newPowerUp);
                    }
                }
            }
            else
            {
                AddNewPowerUp(newPowerUp);
            }
        }
        public bool HasPowerUpType<T>(T type) where T: PowerUpCtrl
        {
            foreach(PowerUpCtrl p_u in powerUps)
            {
                if (p_u.GetType() == type.GetType())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
