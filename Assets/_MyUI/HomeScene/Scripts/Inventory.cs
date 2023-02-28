using PlayerManager;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MzoUI.HomeScene
{
    public class Inventory : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject slotPrefab;
        public CharacterModel[] allCharacters;

        [Header("Character set")]
        public Transform listHolder; //hold all slot (grid layout)
        public CharacterModel characterChoosing;
        [Header("Display")]
        public CharacterDisplay displayer;
        [Header("Select button")]
        public GameObject selectBtn;
        public TMP_Text selectText;
        [Header("Buy button")]
        public AudioClip buySound;
        public GameObject buyBtn;
        public TMP_Text priceText;
        public GameObject reportPanel;
        private void Awake()
        {
            List<string> charactersOwn = DataSystem.AllIdCharacterOwn();
            // check all characters was bought
            foreach(CharacterModel character in allCharacters)
            {
                foreach(string id in charactersOwn)
                {
                    if(character.id == id)
                    {
                        character.wasBought = true;
                        break;
                    }
                }
            }
            foreach (CharacterModel character in allCharacters)
            {
                displayer.InitModel(character);
            }
        }
        private void OnEnable()
        {
            ReloadSlot();
            selectBtn.SetActive(false);
            buyBtn.SetActive(false);
        }
        private void Start()
        {
            displayer.DisplayCharacter(DataSystem.GetIdCharacterSelected());
        }
        public void SetCharacterChoosing(CharacterModel other)
        {
            characterChoosing = other;
            displayer.DisplayCharacter(other.id);
        }
        public void OnOpen()
        {
            // load all characters
            ReloadSlot();
            //
            buyBtn.SetActive(false);
            selectBtn.SetActive(false);
        }
        public void OnClose(GameObject inventoryPanel)
        {
            displayer.DisplayCharacter(DataSystem.GetIdCharacterSelected());
            buyBtn.SetActive(false);
            selectBtn.SetActive(false);
            inventoryPanel.SetActive(false);
        }
        public void EnableSelectBtn(bool wasSelected)
        {
            if (wasSelected)
            {
                selectText.text = "Selected";
                selectBtn.GetComponent<Image>().color = new Color32(213, 231, 119, 255);
            }
            else
            {
                selectText.text = "Select";
                selectBtn.GetComponent<Image>().color = new Color32(139, 231, 119, 255);
            }
            selectBtn.SetActive(true);
            buyBtn.SetActive(false);
        }
        public void EnableBuyBtn(string price)
        {
            priceText.text = price;
            buyBtn.SetActive(true);
            selectBtn.SetActive(false);
        }
        public void OnSelectCharacter()
        {
            // this function for Select character button
            if(characterChoosing.wasBought)
            {
                DataSystem.ChangeCharacterSelected(characterChoosing.id);
            }
            // reload slot
            ReloadSlot();
            EnableSelectBtn(characterChoosing.id == DataSystem.GetIdCharacterSelected());
        }
        public void OnBuy()
        {
            // this function for buy button
            CoinPanel coinPanel = FindObjectOfType<CoinPanel>();
            // buy character
            if (characterChoosing.Buy())
            {
                if (buySound) Audio.instance.source.PlayOneShot(buySound);
                // reload coin
                coinPanel.UpdateCoinAmount();
                // reload slot
                ReloadSlot();
                // can select
                EnableSelectBtn(DataSystem.GetIdCharacterSelected() == characterChoosing.id);
            }
            else
            {
                reportPanel.SetActive(true);
            }
        }
        private void ReloadSlot()
        {
            // remove all old slots
            for(int i=0; i<listHolder.childCount; i++)
            {
                Destroy(listHolder.GetChild(i).gameObject);
            }
            // create slots again
            foreach (CharacterModel character in allCharacters)
            {
                // init slot
                CharacterSlot slot = Instantiate(slotPrefab, listHolder).GetComponent<CharacterSlot>();
                slot.Init(character);
            }
        }
    }
}
