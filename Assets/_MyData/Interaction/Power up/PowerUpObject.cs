using UnityEngine;

namespace Interaction
{
    [CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp", order = 1)]
    public class PowerUpObject: ScriptableObject
    {
        public string nameEffect = "Power up..";
        public Sprite avatar;
        public float effectiveTime = 10f;
    }
}
