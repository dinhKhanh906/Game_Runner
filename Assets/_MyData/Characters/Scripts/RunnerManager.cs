using UnityEngine;
using UnityEngine.Events;
using PlayerManager;

namespace Runner
{
    public class RunnerManager : MonoBehaviour
    {
        public static RunnerManager instance;
        public bool isGameOver;
        public UnityEvent gameOverEvent;
        public UnityEvent revivalEvent;
        public Transform characterHolder;

        private void Awake()
        {
            if (instance) return;
            else instance= this;

            // init character
            InitCharacter();
        }
        private void Start()
        {
            gameOverEvent.AddListener(() =>
            {
                isGameOver = true;
            });
            revivalEvent.AddListener(() =>
            {
                isGameOver = false;
            });
        }
        public bool IsGameOver() => isGameOver;
        private void InitCharacter()
        {
            // remove old character
            for(int i=0; i<characterHolder.childCount; i++)
            {
                Destroy(characterHolder.GetChild(i));
            }
            // init new character
            GameObject character = Instantiate(GameManager.instance.characterSelected, characterHolder);
            // change to running state
            Animator animator = character.GetComponent<Animator>();
            if(animator!= null)
            {
                animator.SetBool("running", true);
            }
        }
    }
}
