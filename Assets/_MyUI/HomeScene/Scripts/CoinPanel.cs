using PlayerManager;
using TMPro;
using UnityEngine;

namespace MzoUI.HomeScene
{
    public class CoinPanel : MonoBehaviour
    {
        public TMP_Text coinAmount;
        private void Awake()
        {
            UpdateCoinAmount();
        }
        public void UpdateCoinAmount()
        {
            // set text
            coinAmount.text = DataSystem.LoadCoinAmount().ToString();
        }
    }
}
