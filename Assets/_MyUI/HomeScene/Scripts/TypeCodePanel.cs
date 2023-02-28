using PlayerManager;
using TMPro;
using UnityEngine;

namespace MzoUI.HomeScene
{
    public class TypeCodePanel : MonoBehaviour
    {
        public TMP_InputField inputField;
        [SerializeField] string code = "mzo280223";
        [SerializeField] int coinsGift = 100000;
        CoinPanel coinPanel;
        private void Awake()
        {
            coinPanel = FindObjectOfType<CoinPanel>();
        }
        public void OnEnter()
        {
            if (inputField.text.Equals(code))
            {
                DataSystem.UpdateCoinAmount(coinsGift);
                coinPanel.UpdateCoinAmount();
            }
        }
    }
}
