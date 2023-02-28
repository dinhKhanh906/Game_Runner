using Runner;
using UnityEngine;
using System.Diagnostics;

namespace Interaction
{
    public class DoubleScorePowerUp : PowerUpCtrl
    {
        public override void Ability()
        {
            PowerUpHolder holder = FindObjectOfType<PowerUpHolder>();
            if (!holder.HasPowerUpType(this))
            {
                Collector collector = FindObjectOfType<Collector>();
                int mul = collector.score.GetMultipler();
                collector.score.SetMultiply( mul * 2);
            }
        }
        public override void ReturnForBeginning()
        {
            base.ReturnForBeginning();
            Collector collector = FindObjectOfType<Collector>();
            int mul = collector.score.GetMultipler();
            collector.score.SetMultiply(mul / 2);
        }
    }
}