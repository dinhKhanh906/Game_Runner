using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Runner;

namespace MzoUI.PlayScene
{
    public class CollectionPanel : MonoBehaviour
    {
        [System.Serializable]
        public class Entity
        {
            [SerializeField] TMP_Text multipleText;
            [SerializeField] TMP_Text amountText;
            public void SetValue(string multiple, string amount)
            {
                multipleText.text = "X" + multiple;
                amountText.text = amount;
            }
        }
        public Collector collector;
        public Entity score;
        public Entity coin;
        private void Awake()
        {
            if (collector == null) collector = FindObjectOfType<Collector>();
        }
        private void Start()
        {
            collector.score.onValueChanged.AddListener(() =>
            {
                score.SetValue(collector.score.GetMultipler().ToString(), collector.score.GetAmount().ToString());
            });

            collector.coins.onValueChanged.AddListener(() =>
            {
                coin.SetValue(collector.coins.GetMultipler().ToString(), collector.coins.GetAmount().ToString());
            });
        }
    }
}
