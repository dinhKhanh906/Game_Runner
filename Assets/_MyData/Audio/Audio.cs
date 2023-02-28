using System.Collections.Generic;
using UnityEngine;

namespace PlayerManager
{
    public class Audio : MonoBehaviour
    {
        public static Audio instance;
        public AudioSource source;
        private void Awake()
        {
            if (instance == null) instance = this;
            if (source == null) source = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            // set volume from save file
            source.volume = DataSystem.GetVolume();
        }
    }
}
