using Runner;

namespace Interaction
{
    public class DoubleCoinPowerUp : PowerUpCtrl
    {
        public override void Ability()
        {
            PowerUpHolder holder = FindObjectOfType<PowerUpHolder>();
            if (!holder.HasPowerUpType(this))
            {
                Collector collector = FindObjectOfType<Collector>();
                int mul = collector.coins.GetMultipler();
                collector.coins.SetMultiply( mul * 2);
            }
        }
        public override void ReturnForBeginning()
        {
            base.ReturnForBeginning();
            Collector collector = FindObjectOfType<Collector>();
            int mul = collector.coins.GetMultipler();
            collector.coins.SetMultiply(mul / 2);
        }
    }
}