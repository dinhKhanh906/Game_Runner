using Runner;

namespace Interaction
{
    public class UncollisionPowerUp : PowerUpCtrl
    {
        Collision runnerCollision;
        public override void Ability()
        {
            runnerCollision = FindObjectOfType<Runner.Collision>();
            runnerCollision.isCheckObstacles = false;
        }
        public override void ReturnForBeginning()
        {
            runnerCollision.isCheckObstacles = true;
        }
    }
}
