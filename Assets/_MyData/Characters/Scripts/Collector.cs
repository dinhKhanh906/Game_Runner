using UnityEngine;
using UnityEngine.Events;
using PlayerManager;

namespace Runner
{
    public class Collector : MonoBehaviour
    {
        [System.Serializable]
        public class Resource
        {
            [HideInInspector] public UnityEvent onValueChanged;
            [SerializeField] int amount;
            [SerializeField] int multiplier = 1;

            public void InsertAmount(int value) 
            {
                amount += value * multiplier;
                onValueChanged.Invoke();
            }
            public void SetMultiply(int value)
            {
                multiplier = value;
                onValueChanged.Invoke();
            }
            public int GetAmount() { return amount; }
            public int GetMultipler() { return multiplier; }
        }
        [Header("Score")]
        public Resource score;
        public float deltaScoreUp = 0.5f;
        private float timerScoreUp = 0f;
        [Header("Coins")]
        public Resource coins;

        [SerializeField] Collision collsion;

        private void Start()
        {
            if(collsion == null) collsion = FindObjectOfType<Collision>();
            coins.SetMultiply(1);
            score.SetMultiply(1);
            RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                score.SetMultiply(0);
                coins.SetMultiply(0);
                SaveResources();
            });
            RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                score.SetMultiply(1);
                coins.SetMultiply(1);
            });
        }
        private void Update()
        {
            timerScoreUp -= Time.deltaTime;
            if(timerScoreUp <= 0f)
            {
                score.InsertAmount(1);
                timerScoreUp = deltaScoreUp;
            }
            Collect();
        }
        public void Collect()
        {
            Interaction.Interaction interaction = collsion.GetInteractionForward();
            if (interaction)
            {
                interaction.ActiveAbility();
            }
        }
        public void SaveResources()
        {
            // save coin
            Debug.Log("update coin");
            DataSystem.UpdateCoinAmount(coins.GetAmount());
        }
    }

}