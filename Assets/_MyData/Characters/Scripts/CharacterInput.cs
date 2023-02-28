using UnityEngine;

namespace Runner
{
    public class CharacterInput : MonoBehaviour
    {
        [SerializeField] Movement movement;
        public bool hasListen;
        private void Awake()
        {
            hasListen = false;
        }
        private void Start()
        {
            if(!movement) movement = FindObjectOfType<Movement>();
            RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                hasListen = false;
            });
            RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                hasListen = true;
            });
        }
        private void Update()
        {
            if (hasListen == false) return;

            if (Input.GetKeyDown(KeyCode.RightArrow)) movement.OnChangeLane(1);
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) movement.OnChangeLane(-1);

            if(Input.GetKeyDown(KeyCode.UpArrow)) movement.OnJump();
            else if(Input.GetKeyDown(KeyCode.DownArrow)) movement.OnSlide();
        }
    }
}
