using MzoUI.HomeScene;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject characterSelected;
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public void SetUpForPlayScene()
        {
            Inventory inventory = FindObjectOfType<Inventory>();
            CharacterModel[] allCharacter = inventory.allCharacters;
            string idChecking = DataSystem.GetIdCharacterSelected();
            foreach(CharacterModel character in allCharacter)
            {
                if(character.id == idChecking)
                {
                    characterSelected = character.model;
                    break;
                }
            }
        }
    }
}
