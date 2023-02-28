using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayerManager;
using UnityEngine.TextCore.Text;

namespace MzoUI.HomeScene
{
    public class CharacterDisplay : MonoBehaviour
    {
        [System.Serializable]
        public class ModelDisplay
        {
            public string id;
            public GameObject body;
        }
        public float rotateSpeed = 50f;
        public Transform characterHolder;
        public ModelDisplay modelDisplaying;
        public List<ModelDisplay> allModel; 
        // Update is called once per frame
        void Update()
        {
            // animate, rotate model around
            characterHolder.Rotate(Vector3.up, rotateSpeed*Time.deltaTime);
        }
        public void InitModel(CharacterModel character)
        {
            GameObject body = Instantiate(character.model, characterHolder);
            body.SetActive(false);
            allModel.Add(new ModelDisplay() { body = body, id = character.id });
        }
        public void DisplayCharacter(string id)
        {
            foreach(ModelDisplay model in allModel)
            {
                if(model.id == id)
                {
                    modelDisplaying = model;
                    model.body.SetActive(true);
                }
                else
                {
                    model.body.SetActive(false);
                }
            }
        }
    }
}
