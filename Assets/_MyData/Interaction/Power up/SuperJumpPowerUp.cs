using Runner;

namespace Interaction
{
    public class SuperJumpPowerUp : PowerUpCtrl
    {
        Movement movement;
        public float percentUp = 1.5f;
        public override void Ability()
        {
            PowerUpHolder holder = FindObjectOfType<PowerUpHolder>();
            movement = FindObjectOfType<Movement>();
            if (!holder.HasPowerUpType(this))
            {
                movement.jumpForce *= percentUp;
            }
        }

        public override void ReturnForBeginning()
        {
            base.ReturnForBeginning();
            movement.jumpForce /= percentUp;
        }
    }
}
