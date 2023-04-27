using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelReader : MonoBehaviour
{
    [SerializeField] private TextAsset textAsset;
    [SerializeField] private GameLogic _gameLogic;
    [SerializeField] TextAsset _levelFile;

    void Start()
    {
        LoadLevel(textAsset);
        //string json = _levelFile.text;
        //StandConfigData levelData = JsonConv
    }

    public void LoadLevel(TextAsset textAsset)
    {
        string[] lines = textAsset.text.Split(new string[] {"\n", "\r"}, System.StringSplitOptions.RemoveEmptyEntries);

        List<int[]> boughArrays = new List<int[]>();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if(i == 0)
            {
                string[] line0Splits = line.Split(new char[] {','}, System.StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                int[] convertArray = new int[4];
                //line[char]
                for (int j = 0; j < convertArray.Length; j++)
                {
                    convertArray[j] = CharacterToInt(line[j]);
                }

                boughArrays.Add(convertArray);
            }
        } 

        _gameLogic.LoadLevel(boughArrays);
    }

    private int CharacterToInt(char c)
    {
        switch (c)
        {
            default: return 0;

            case '0': return 0;
            case '1': return 1;
            case '2': return 2;
            case '3': return 3;
            case '4': return 4;
            case '5': return 5;
            case '6': return 6;
            case '7': return 7;
            case '8': return 8;
            case '9': return 9;
            case 'A': return 10;
        }
    }
}
