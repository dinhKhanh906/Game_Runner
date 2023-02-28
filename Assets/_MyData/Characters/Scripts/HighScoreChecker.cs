using UnityEngine;
using UnityEngine.UI;
using PlayerManager;
using TMPro;
using MzoUI;

namespace Runner
{
    public class HighScoreChecker : MonoBehaviour
    {
        [Header("UI")]
        public TMP_Text tmpNameComparing;
        public TMP_Text tmpScoreComparing;
        public TMP_InputField nameField;
        public GameObject panelReport;
        public GameObject newHighScorePanel;
        [Header("properties")]
        public HighScoreList highScoreList;
        public Collector collector;
        public int currentRank;
        public bool hasNewHighScore = false;
        [SerializeField] HighScore scoreComparing;
        private void Awake()
        {
            highScoreList = DataSystem.LoadAllHighScore();
            currentRank = highScoreList.list.Length; // no rank at begin
            Debug.Log("Current rank " + currentRank);
            if(collector == null) collector = FindObjectOfType<Collector>();
        }
        private void Start()
        {
            newHighScorePanel.SetActive(false);
            panelReport.SetActive(false);
        }
        private void Update()
        {
            CompareScore();
        }
        public void UpDateHighScore()
        {
            // update
            if (string.IsNullOrEmpty(nameField.text))
            {
                // check name input
                panelReport.SetActive(true);
                return; 
            }

            HighScore newHighScore = new HighScore() { name = nameField.text.Trim(), score = collector.score.GetAmount() };
            DataSystem.SaveHighScore(newHighScore);
            // back home scene
            Loading loading = FindObjectOfType<Loading>();
            loading.OnLoadScene("HomeScene");
        }
        private void CompareScore()
        {
            if (currentRank == 0) return; // now, score is top 1, no compare
            //
            scoreComparing = DataSystem.LoadHighScore(currentRank - 1); // allways compare score higher 1 level
            tmpNameComparing.text = scoreComparing.name;
            tmpScoreComparing.text = scoreComparing.score.ToString();
            if (collector.score.GetAmount() > scoreComparing.score)
            {
                currentRank--; // up rank
                hasNewHighScore = true;
            }
        }
        public void OnRetypeName()
        {
            panelReport.SetActive(false);
        }
        public void OnOpenPanel()
        {
            if (!hasNewHighScore) return;

            newHighScorePanel.SetActive(true);
        }
    }
}
