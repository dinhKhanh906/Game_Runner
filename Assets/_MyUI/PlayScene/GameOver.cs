using TMPro;
using UnityEngine;
using Runner;
using PlayerManager;
using System.Collections;

namespace MzoUI.PlayScene
{
    public class GameOver : MonoBehaviour
    {
        public GameObject gameOverPanel;
        public TMP_Text tmpTimer;
        public float timer;
        [Header("Continue action set up")]
        public GameObject continuePanel;
        public TMP_Text tmpCoinRequire;
        public int coinRequire;

        bool isWaiting;
        HighScoreChecker highScoreChecker;
        private void Awake()
        {
            tmpTimer.text = timer.ToString();
            tmpCoinRequire.text = coinRequire.ToString();
            highScoreChecker = FindObjectOfType<HighScoreChecker>();
        }
        private void Start()
        {

            RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                gameOverPanel.SetActive(true);
                if (DataSystem.LoadCoinAmount() < coinRequire) continuePanel.SetActive(false);
                isWaiting = true;
                StartCoroutine(CountDown());
            });
            gameOverPanel.SetActive(false);
        }
        public void OnGiveUp()
        {
            if (highScoreChecker.hasNewHighScore)
            {
                highScoreChecker.OnOpenPanel();
                isWaiting = false;
            }
            else
            {
                Loading loading = FindObjectOfType<Loading>();
                loading.OnLoadScene("HomeScene");
            }
 
        }
        public void OnContinue()
        {
            // reduce coin
            DataSystem.UpdateCoinAmount(-coinRequire);
            // continue game
            RunnerManager.instance.revivalEvent.Invoke();
            gameOverPanel.SetActive(false);
            isWaiting = false;
        }
        IEnumerator CountDown()
        {
            int deltaSec = 1; // 1 second
            float timeRemain = timer;
            while(timeRemain > 0 && isWaiting)
            {
                yield return new WaitForSeconds(deltaSec);
                timeRemain -= deltaSec;
                tmpTimer.text = timeRemain.ToString();
            }
            // after wait too long --> really die
            if (isWaiting)
            {
                if (highScoreChecker.hasNewHighScore)
                {
                    highScoreChecker.OnOpenPanel();
                }
                else
                {
                    RunnerManager.instance.isGameOver = true;
                    FindObjectOfType<Loading>().OnLoadScene("HomeScene");
                }
            }
        }
    }
}
