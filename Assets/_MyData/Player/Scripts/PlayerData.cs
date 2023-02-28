using UnityEngine;

namespace PlayerManager
{
    [System.Serializable]
    public class PlayerData
    {
        public float volume;
        public HighScoreList highScoreList;
        public CoinStorage coinStorage;
        public CharacterStorage character;
    }
}
