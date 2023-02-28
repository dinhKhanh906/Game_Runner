using UnityEngine;

namespace PlayerManager
{
    [CreateAssetMenu(fileName = "Character model", menuName = "ScriptableObjects/Character model", order = 1)]
    public class CharacterModel : ScriptableObject
    {
        public Sprite avatar;
        public string id;
        public GameObject model;
        public int price;
        public bool wasBought;
        public bool Buy()
        {
            // check was bought or not
            if (wasBought || DataSystem.LoadCoinAmount() < price) return false;

            DataSystem.UpdateCoinAmount(-price);
            DataSystem.AddNewCharacterOwn(id);
            wasBought = true;
            return true;
        }
    }
}
