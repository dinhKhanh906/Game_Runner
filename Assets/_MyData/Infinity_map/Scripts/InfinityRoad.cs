
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityRoad
{
    public class InfinityRoad : MonoBehaviour
    {
        [Header("Road prefab")]
        [SerializeField] GameObject[] prefabs;
        [SerializeField] GameObject startRoadPrefab;
        [Header("Roads when start game")]
        [SerializeField] int amountStart = 1;
        [Header("Roads actived")]
        [SerializeField] RoadController roadLatest;
        [SerializeField] int maxActived = 6;
        public int countActived;

        [Header("queue waiting")]
        [SerializeField] int maxWaiting = 10;
        [SerializeField] List<GameObject> roadsReady;
        private Queue<GameObject> roadsWaiting;
        private void Awake()
        {
            countActived = 0;
            roadsWaiting = new Queue<GameObject>();
            // Init roadsReady list
            roadsReady = new List<GameObject>();
            foreach (GameObject prefab in prefabs)
            {
                GameObject p = Instantiate(prefab, transform);
                p.SetActive(false);
                roadsReady.Add(p);
            }
        }
        private void Start()
        {
            // Spawn start roads when start game
            SpawnStartRoads(amountStart);
            //
            for (int i = countActived; i < maxActived; i++)
            {
                Spawn();
            }
        }
        public void Spawn()
        {
            if (countActived >= maxActived) return;

            // random index road to spawn
            int index = UnityEngine.Random.Range(0, roadsReady.Count);

            // Active road
            ActiveRoad(index);
            countActived++;
        }
        private void SpawnStartRoads(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                // init new road
                RoadController newRoad = Instantiate(startRoadPrefab, transform).GetComponent<RoadController>();

                // Active road
                newRoad.gameObject.SetActive(true);
                newRoad.transform.position = roadLatest != null ? Vector3.forward * (roadLatest.GetLastPositionZ() + (newRoad.GetLength() / 2) - 0.3f) : Vector3.forward * -20;

                // set road latest
                roadLatest = newRoad;

                // destroy this road after 20 seconds
                Destroy(newRoad.gameObject, 20f);
            }
        }
        private void ActiveRoad(int index)
        {
            RoadController newRoad = roadsReady[index].GetComponent<RoadController>();
            newRoad.ResetValue();
            // Active road
            newRoad.gameObject.SetActive(true);
            newRoad.transform.position = roadLatest != null ? Vector3.forward * (roadLatest.GetLastPositionZ() + (newRoad.GetLength() / 2) - 0.3f) : Vector3.forward * -20;

            // set road latest
            roadLatest = newRoad.GetComponent<RoadController>();

            // Add into waiting queue
            roadsWaiting.Enqueue(newRoad.gameObject);
            roadsReady.Remove(newRoad.gameObject);

            // move the last road in waiting to ready if waiting is max
            if (roadsWaiting.Count > maxWaiting)
            {
                GameObject lastWaiting = roadsWaiting.Dequeue();
                roadsReady.Add(lastWaiting);
            }
        }
    }

}