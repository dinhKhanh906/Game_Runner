using UnityEngine;
using PlayerManager;
using TMPro;

namespace MzoUI.HomeScene
{
    public class Rank: MonoBehaviour
    {
        public GameObject rankPanel;
        public GameObject playerSlot;
        public Transform listHolder;
        private void Awake()
        {
            AddPlayerSlots();
            rankPanel.SetActive(false);
        }
        private void AddPlayerSlots()
        {
            HighScoreList highScoreList = DataSystem.LoadAllHighScore();
            Debug.Log(highScoreList.list[0].name);
            for(int i=0; i<highScoreList.list.Length; i++)
            {
                HighScore player = highScoreList.list[i];
                if (!string.IsNullOrEmpty(player.name))
                {
                    GameObject newSlot = Instantiate(playerSlot, listHolder);
                    TMP_Text tmp = newSlot.GetComponent<TMP_Text>();
                    tmp.text =  (i+1) + "| " + player.name + ": " + player.score.ToString();
                }
            }
            
        }
        public void OnOpen()
        {
            rankPanel.SetActive(true);
        }
        public void OnClose()
        {
            rankPanel.SetActive(false);
        }
    }
}
