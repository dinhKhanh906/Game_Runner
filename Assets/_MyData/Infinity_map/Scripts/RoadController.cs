
using System.Collections.Generic;
using UnityEngine;
using Interaction;

namespace InfinityRoad
{
    public class RoadController : MonoBehaviour
    {
        CoinPool coinPool;
        InfinityRoad infinityRoad;
        PowerUpSpawner powerUpSpawner;
        int spawnCount;

        [SerializeField] float length = 30f;
        [SerializeField] List<Transform> coinSpawnPoints;
        [SerializeField] List<Transform> powerUpSpawnPoints;
        Queue<int> indexsCoinNeedRemove;
        private void Reset()
        {
            SetCoinSpawnPoints(transform);
            SetPowerUpSpawnPoints(transform);
        }
        private void Awake()
        {
            infinityRoad = FindObjectOfType<InfinityRoad>();
            coinPool = FindObjectOfType<CoinPool>();
            powerUpSpawner = FindObjectOfType<PowerUpSpawner>();
            indexsCoinNeedRemove = new Queue<int>();
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                transform.position -= transform.forward * RoadManager.instance.speed * Time.deltaTime;

                if (this.GetLastPositionZ() < -10f)
                {
                    Deactive();
                }
            }
        }
        private void SetCoinSpawnPoints(Transform parrentObject)
        {
            if (parrentObject.childCount <= 0) return;

            for (int i = 0; i < parrentObject.transform.childCount; i++)
            {
                Transform child = parrentObject.GetChild(i);
                if (child.tag == "CoinPoint" && !coinSpawnPoints.Contains(child))
                {
                    coinSpawnPoints.Add(child);
                }
                else SetCoinSpawnPoints(child);
            }
        }
        private void SetPowerUpSpawnPoints(Transform parrentObject)
        {
            if (parrentObject.childCount <= 0) return;

            for (int i = 0; i < parrentObject.transform.childCount; i++)
            {
                Transform child = parrentObject.GetChild(i);
                if (child.tag == "PowerUp" && !powerUpSpawnPoints.Contains(child))
                {
                    powerUpSpawnPoints.Add(child);
                }
                else SetPowerUpSpawnPoints(child);
            }
        }
        public void ResetValue()
        {
            Debug.Log(gameObject.name + "|" + spawnCount++);
            // reset value
            // clear old values
            foreach(Transform pu in powerUpSpawnPoints)
            { 
                // respawn
                powerUpSpawner.Spawn(pu);
            }
            // take coins & set position for each coin
            if (coinSpawnPoints.Count <= 0) return;
            for (int i=0; i<coinSpawnPoints.Count; i++)
            {
                Coin coin = coinPool.UseCoin();
                coin.SetTransform(coinSpawnPoints[i]);
                coin.gameObject.SetActive(true);
            }
        }
        private void Deactive()
        {

            // spawn another road
            infinityRoad.Spawn();

            foreach (Transform pu in powerUpSpawnPoints)
            {
                // remove old item
                if (pu.childCount > 0) Destroy(pu.GetChild(0).gameObject);
            }
            // return all coins remain to CoinPool
            foreach (Transform coinTrans in coinSpawnPoints)
            {
                Coin coin = coinTrans.GetComponentInChildren<Coin>();
                coinPool.DeactiveCoin(coin);
            }
            infinityRoad.countActived--;

            // deactive this road
            gameObject.SetActive(false);
        }
        public float GetLastPositionZ()
        {
            // sum of center point and length/2
            return transform.position.z + length / 2;
        }
        public float GetLength() => length;
    }

}