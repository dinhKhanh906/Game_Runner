using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerManager
{
    public class DataSystem
    {
        public static string dataPath = Application.dataPath + "/dataPlayer.json";
        public static bool UpdateCoinAmount(int amount)
        {
            CoinStorage allCoins = new CoinStorage() { amount = amount + LoadCoinAmount()};
            PlayerData playerData = ReadData();
            // update values
            playerData.coinStorage = allCoins;
            // update
            return WriteFile(playerData);
        }
        public static int LoadCoinAmount()
        {
            // get all coins
            return ReadData().coinStorage.amount;
        }
        public static bool SaveHighScore(HighScore newHighScore)
        {
            HighScoreList highScoreList = LoadAllHighScore();
            // create new array bigger 1 size
            HighScore[] newList = new HighScore[highScoreList.list.Length + 1];
            for(int i=0; i<newList.Length; i++)
            {
                if(i == newList.Length-1) newList[i] = newHighScore;
                else newList[i] = highScoreList.list[i];
            }
            // sort high score (low -> high)
            for (int i = 0; i < newList.Length; i++)
            {
                for (int j = i + 1; j < newList.Length; j++)
                {
                    if (newList[j].score <= newList[i].score) continue;
                    // swap position in array
                    HighScore saveValue = newList[i];
                    newList[i] = newList[j];
                    newList[j] = saveValue;
                }
            }
            // covert to new list
            for(int i=0; i<highScoreList.list.Length; i++)
            {
                highScoreList.list[i] = newList[i];
            }
            // save them
            PlayerData data = ReadData();
            data.highScoreList = highScoreList;
            return WriteFile(data);
        }
        public static HighScore LoadHighScore(int index)
        {
            // get a high score
            return ReadData().highScoreList.list[index];
        }
        public static HighScoreList LoadAllHighScore()
        {
            // return all high scores
            return ReadData().highScoreList;
        }
        public static bool ChangeCharacterSelected(string id)
        {
            PlayerData data = ReadData();
            // set new value
            data.character.idSelected = id;
            // update data
            return WriteFile(data);
        }
        public static string GetIdCharacterSelected()
        {
            return ReadData().character.idSelected;
        }
        public static bool AddNewCharacterOwn(string id)
        {
            PlayerData data = ReadData();
            data.character.allIdOwn.Add(id);
            return WriteFile(data);
        }
        public static List<string> AllIdCharacterOwn()
        {
            return ReadData().character.allIdOwn;
        }
        public static float GetVolume()
        {
            return ReadData().volume;
        }
        public static bool SetVolume(float value)
        {
            PlayerData data = ReadData();
            data.volume = value;

            return WriteFile(data);
        }
        public static PlayerData ReadData()
        {
            PlayerData playerData = new PlayerData();
            if (File.Exists(dataPath))
            {
                // read json file
                string jsonValue = File.ReadAllText(dataPath);
                playerData = JsonUtility.FromJson<PlayerData>(jsonValue);
            }
            else
            {
                // init value start
                HighScoreList highScoreList = new HighScoreList() { list = new HighScore[5] };
                CoinStorage coinStorage = new CoinStorage();
                CharacterStorage characterStorage = new CharacterStorage() { idSelected = "mA", allIdOwn = new List<string>() { "mA"} };
                float volume = 0.5f;
                playerData = new PlayerData() { volume = volume, character = characterStorage, coinStorage = coinStorage, highScoreList = highScoreList};
                // create new file
                string jsonValue = JsonUtility.ToJson(playerData);
                File.WriteAllText(dataPath, jsonValue);
            }

            return playerData;
        }
        public static bool WriteFile(PlayerData data)
        {
            string jsonValue = JsonUtility.ToJson(data);
            // save data
            try
            {
                File.WriteAllText(dataPath, jsonValue);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        }
    }
}
