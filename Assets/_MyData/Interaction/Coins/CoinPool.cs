using System.Collections.Generic;
using UnityEngine;

namespace Interaction
{
    public class CoinPool : MonoBehaviour
    {
        public GameObject prefab;
        public int max;
        [SerializeField] List<Coin> coinsUsing;
        [SerializeField] Queue<Coin> coinsReady;
        private void Awake()
        {
            coinsReady = new Queue<Coin>();
            coinsUsing = new List<Coin>();

            for (int i = 0; i < max; i++)
            {
                // instantiate new coin from prefab
                GameObject newCoin = Instantiate(prefab, Vector3.down * 50f, Quaternion.identity, transform);
                // add coins ready queue
                coinsReady.Enqueue(newCoin.GetComponent<Coin>());
            }
        }
        public Coin UseCoin()
        {
            if (coinsReady.Count <= 0) return null;

            Coin coin = coinsReady.Dequeue();
            coinsUsing.Add(coin);
            return coin;
        }
        public void DeactiveCoin(Coin coin)
        {
            if (coin != null)
            {
                coin.SetTransform(transform);
                coinsUsing.Remove(coin);
                coinsReady.Enqueue(coin);
                // deactive
                coin.gameObject.SetActive(false);
            }
        }
    }

}