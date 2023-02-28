using PlayerManager;
using UnityEngine;
using UnityEngine.UI;

namespace MzoUI.HomeScene
{
    public class CharacterSlot : MonoBehaviour
    {
        // data
        public CharacterModel character;
 
        // display
        public Image avatar;
        public GameObject checkWasBought;
        public void Init(CharacterModel data)
        {
            character = data;
            if(data.avatar) avatar.sprite = data.avatar;
            //
            if(DataSystem.AllIdCharacterOwn().Contains(data.id)) checkWasBought.SetActive(true);
            else checkWasBought.SetActive(false);
        }
        public void OnChooseSlot()
        {
            Inventory inventory = FindObjectOfType<Inventory>();

            inventory.SetCharacterChoosing(character);
            // display state of model
            if (character.wasBought)
            {
                // was bought --> can select
                inventory.EnableSelectBtn(DataSystem.GetIdCharacterSelected() == character.id);
            }
            else
            {
                // must buy
                inventory.EnableBuyBtn(character.price.ToString());
            }
        }
    }
}
