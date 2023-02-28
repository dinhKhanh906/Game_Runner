
using System.Collections.Generic;

namespace PlayerManager
{
    [System.Serializable]
    public class HighScore
    {
        public string name;
        public int score;
    }
    [System.Serializable]
    public class HighScoreList 
    {
        public HighScore[] list;
    }
}
