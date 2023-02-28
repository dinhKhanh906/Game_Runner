using UnityEngine;
using Runner;
namespace Interaction
{
    public class Coin : Interaction
    {
        CoinPool coinPool;
        Collector runnerCollector;
        private void Awake()
        {
            coinPool = GameObject.FindObjectOfType<CoinPool>();
            runnerCollector = FindObjectOfType<Collector>();
        }
        public override void ActiveAbility()
        {
            base.ActiveAbility();
            // insert coin to storage
            runnerCollector.coins.InsertAmount(1);
            // deactive
            // insert to coinsReady queue
            coinPool.DeactiveCoin(this);
        }
        public override void SetTransform(Transform parent)
        {
            transform.position = parent.position;
            transform.parent = parent;
        }
    }

}