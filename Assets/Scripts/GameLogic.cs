using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    [SerializeField] private GameGraphic gameGraphic;

    public List<Bough> boughes;

    #region MAIN
    public void LoadLevel(List<int[]> listArray)
    {
        boughes = new List<Bough>();

        foreach (int[] arr in listArray)
        {
            Bough b = new Bough();

            for (int i = 0; i < arr.Length; i++)
            {
                int element = arr[i];
                if (element == 0)
                    continue;

                b.birds.Add(new Bird
                {
                    type = element
                });
            }
            boughes.Add(b);
        }

        gameGraphic.CreateBough(boughes);
        //gameGraphic.CreateBoughRight(boughes);

    }
    private void Update()
    {
        bool isWin = CheckWinCondition();
        //Debug.Log("Is Win: " + isWin);
    }

    //public void PrintBoughes()
    //{
    //    Debug.Log("Boughes======");
    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < boughes.Count; i++)
    //    {
    //        Bough bough = boughes[i];
    //        sb.Append("Bought " + (i+1) + ":");
    //        foreach (Bird bird in bough.birds)
    //        {
    //            sb.Append("" + bird.type);
    //            sb.Append(",");
    //        }
    //        Debug.Log(sb.ToString());
    //        sb.Clear();
    //    }

    //    bool isWin = CheckWinCondition();
    //    Debug.Log("Is Win: " + isWin);
    //}

    public void SwitchBird(Bough bough1, Bough bough2)
    {
        List<Bird> bough1Birds = bough1.birds;
        List<Bird> bough2Birds = bough2.birds;

        if (bough1Birds.Count == 0)
            return;

        if (bough2Birds.Count == 4)
            return;

        int index = bough1Birds.Count - 1;
        Bird b = bough1Birds[index];

        var type = b.type;

        if (bough2Birds.Count > 0 && bough2Birds[bough2Birds.Count - 1].type != type)
        {
            return;
        }

        for (int i = index; i >= 0; i--)
        {
            Bird bird = bough1Birds[i];
            if (bird.type == type)
            {
                bough1Birds.RemoveAt(i);
                bough2Birds.Add(bird);

                if(bough2Birds.Count == 4)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    public void SwitchBird(int boughIndex1, int boughIndex2)
    {
        Bough b1 = boughes[boughIndex1];
        Bough b2 = boughes[boughIndex2];

        SwitchBird(b1, b2);

        gameGraphic.RefreshBough(boughes);
    }

    public List<SwithBirdCommand> CheckSwithBird(int boughIndex1, int boughIndex2)
    {
        List<SwithBirdCommand> commands = new List<SwithBirdCommand>();

        Bough bough1 = boughes[boughIndex1];
        Bough bough2 = boughes[boughIndex2];

        List<Bird> bough1Birds = bough1.birds;
        List<Bird> bough2Birds = bough2.birds;

        if (bough1Birds.Count == 0)
            return commands;

        if (bough2Birds.Count == 4)
        {
            return commands;
        }    

        int index = bough1Birds.Count - 1;
        Bird b = bough1Birds[index];

        var type = b.type;

        if (bough2Birds.Count > 0 && bough2Birds[bough2Birds.Count - 1].type != type)
        {
            return commands;
        }

        int targetIndex = bough2Birds.Count;

        for (int i = index; i >= 0; i--)
        {
            Bird bird = bough1Birds[i];
            if (bird.type == type)
            {
                //bough1Birds.RemoveAt(i);
                //bough2Birds.Add(bird);

                int fromBirdIndex = i;
                int toBirdIndex = targetIndex;
                int fromBoughIndex = boughIndex1;
                int toBoughIndex = boughIndex2;

                commands.Add(new SwithBirdCommand
                {
                    type = type,
                    fromBirdIndex = fromBirdIndex,
                    toBirdIndex = toBirdIndex,
                    fromBoughIndex = fromBoughIndex,
                    toBoughIndex = toBoughIndex,
                });

                targetIndex++;

                if (targetIndex == 4)
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }

        return commands;
    }

    public bool CheckWinCondition()
    {
        bool isWin = true;

        foreach (Bough bough in boughes)
        {
            if (bough.birds.Count == 0)
                continue;

            if(bough.birds.Count < 4)
            {
                isWin = false;
                break;
            }

            bool sameTypeFlag = true;
            int type = bough.birds[0].type;
            foreach (Bird bird in bough.birds)
            {
                if(bird.type != type)
                {
                    sameTypeFlag = false;
                    break;
                }
            }
            if (!sameTypeFlag) 
            {
                isWin = false; break;
            }
        }
        return isWin;
    }

    public class SwithBirdCommand
    {
        public int type;

        public int fromBoughIndex;
        public int fromBirdIndex;
        public int toBoughIndex;
        public int toBirdIndex;
    }
    public class Bough
    {
        public List<Bird> birds = new List<Bird>();
    }

    public class Bird
    {
        public int type;
    }

    #endregion
}
