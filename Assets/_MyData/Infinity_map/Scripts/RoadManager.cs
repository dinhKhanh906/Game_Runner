using UnityEngine;

namespace InfinityRoad
{
    public class RoadManager : MonoBehaviour
    {
        public static RoadManager instance;
        public float deltaGrowUp = 0.02f; // once per second
        float deltaGrowUpSaver;
        public float speed = 3;
        float speedSaver;
        private void Awake()
        {
            if (instance) return;
            else instance= this;
        }
        private void Start()
        {
            //Grow up
            InvokeRepeating("GrowUpSpeed", 1f, 1f);

            Runner.RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                speedSaver = speed;
                deltaGrowUpSaver = deltaGrowUp;
                speed = 0;
                deltaGrowUp = 0;
            });
            Runner.RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                speed = speedSaver;
                deltaGrowUp = deltaGrowUpSaver;
            });
        }
        private void GrowUpSpeed()
        {
            speed += deltaGrowUp;
        }
    }

}