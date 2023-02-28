using System.Collections;
using UnityEngine;

namespace Runner
{
    public class StalkerController : MonoBehaviour
    {
        public Transform stalker;
        public Transform runner;
        public float moveSpeed;
        public Animator animator;
        public Vector3 targetOffset;
        [SerializeField] Vector3 startPoint;
        private void Awake()
        {
            startPoint = stalker.position;
            if (runner == null) runner = FindObjectOfType<Movement>().transform;
            if (animator == null) animator = GetComponentInChildren<Animator>();
        }
        private void Start()
        {
            RunnerManager.instance.gameOverEvent.AddListener(() =>
            {
                AttackRunner();
            });
            RunnerManager.instance.revivalEvent.AddListener(() =>
            {
                BackStart();
            });
        }
        private void Update()
        {
            stalker.LookAt(runner.position);
        }
        public void AttackRunner()
        {
            // move toward to runner

            // bite runner
            Vector3 targetPosition = new Vector3((runner.position + targetOffset).x, startPoint.y, (runner.position + targetOffset).z);
            StartCoroutine(Attack(targetPosition));
        }
        IEnumerator Attack(Vector3 targetPosition)
        {
            animator.SetBool("gameOver", true);
            while(stalker.position != targetPosition)
            {
                stalker.position = Vector3.MoveTowards(stalker.position, targetPosition, moveSpeed*Time.deltaTime);
                yield return null;
            }
            // attack runner
            animator.Play("Attack");
        }
        public void BackStart()
        {
            animator.SetBool("gameOver", false);
            stalker.position = startPoint;
        }
    }
}
