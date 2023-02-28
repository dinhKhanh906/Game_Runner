using PlayerManager;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class Interaction : MonoBehaviour
    {
        public AudioClip soundEff;
        protected virtual void Start()
        {
            SphereCollider collider = GetComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 0.3f;

            gameObject.layer = LayerMask.NameToLayer("Interaction");
        }
        // ability..
        public virtual void ActiveAbility()
        {
            if(soundEff!= null)
            {
                // play sound effect
                Audio.instance.source.PlayOneShot(soundEff);
            }
            gameObject.SetActive(false);
        }
        // set specific position
        public abstract void SetTransform(Transform parent);
    }
}
