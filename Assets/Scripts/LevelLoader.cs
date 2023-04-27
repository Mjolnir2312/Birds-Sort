using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [System.Serializable]
    public class StandConfig
    {
        public int[] idBirds;
        public StandData standData;
        public int side;
    }

    [System.Serializable]
    public class StandData
    {
        public int type;
        public int numSlot;
    }

    [System.Serializable]
    public class LevelConfig
    {
        public StandConfig[] standConfig;
    }

    private void Start()
    {
        string path = @"D:\SourceGame\BirdSort\Assets\Resources\levels\Level_2.txt";
        string jsonString = File.ReadAllText(path);
        LevelConfig levelConfig = JsonUtility.FromJson<LevelConfig>(jsonString);

        foreach (StandConfig standConfig in levelConfig.standConfig)
        {
            Debug.Log("idBirds: " + string.Join(",", standConfig.idBirds)
                + ", type: " + standConfig.standData.type + ", numSlot: " + standConfig.standData.numSlot + ", side: " + standConfig.side);
        }
    }
}
