using UnityEngine;
using TMPro;
namespace Runner
{
    public class TextNotice : MonoBehaviour
    {
        public TMP_Text tmp;
        public Animator animator;
        private void Awake()
        {
            if(tmp==null) tmp = GetComponent<TMP_Text>();
            if(animator==null) animator = GetComponent<Animator>();
        }
        private void Start()
        {
            gameObject.SetActive(true);
        }
        public void OnActive(string notice)
        {
            tmp.text = notice;
            gameObject.SetActive(true);
            animator.Play("TextNotice");
        }
        public void OnDeactive()
        {
            Debug.Log("deactived");
            gameObject.SetActive(false);
        }
    }
}
