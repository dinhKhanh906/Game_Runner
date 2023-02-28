using Runner;
using UnityEngine;

namespace Interaction
{
    public abstract class PowerUpCtrl : Interaction
    {
        public PowerUpObject data;
        public float effectiveTime = 0f;
        public bool wasReturn;
        protected override void Start()
        {
            base.Start();
            this.effectiveTime = data.effectiveTime;
        }
        public Sprite GetImage()
        {
            return data.avatar;
        }
        public override void ActiveAbility()
        {
            base.ActiveAbility();
            // power up
            Ability();
            // insert into PowerUpHolder
            PowerUpHolder powerUpHolder = FindObjectOfType<PowerUpHolder>();
            powerUpHolder.Insert(this);
            wasReturn = false;
        }
        public abstract void Ability();
        public virtual void ReturnForBeginning()
        {
            wasReturn= true;
        }
        public override void SetTransform(Transform parent)
        {
            throw new System.NotImplementedException();
        }
    }
}