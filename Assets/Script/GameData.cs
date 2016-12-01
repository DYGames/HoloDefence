using UnityEngine;
using System.Collections;

public class GameData
{
    public static GameData Instance
    {
        get
        {
            if(Instance == null)
            {
                Instance = new GameData();
            }
            return Instance;
        }
        private set
        {
            Instance = value;
        }
    }

    GameData()
    {
        
    }

}
