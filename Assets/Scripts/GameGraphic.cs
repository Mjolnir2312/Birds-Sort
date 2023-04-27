using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameLogic;

public class GameGraphic : MonoBehaviour
{
    [SerializeField] Vector3 boughStartPositionLeft;
    [SerializeField] Vector3 boughStartPositionRight;
    [SerializeField] Vector3 boughDistance;
    [SerializeField] private Bird birdGraphic;
    [SerializeField] Bough prefabBoughLeft;
    [SerializeField] Bough prefabBoughRight;

    private GameLogic _game;
    private Bird previewBird;
    private bool isSwichingBall = false;

    public int selectBoughIndex = -1;
    public List<Bough> boughGrap;

    private void Start()
    {
        _game = FindObjectOfType<GameLogic>();
        selectBoughIndex = -1;

        previewBird = Instantiate(birdGraphic);
    }

    public void CreateBough(List<GameLogic.Bough> boughs)
    {
        foreach(GameLogic.Bough b in boughs)
        {
            Bough bgL = Instantiate(prefabBoughLeft);

            boughGrap.Add(bgL);

            List<int> birdTypes = new List<int>();

            foreach (var bird in b.birds)
            {
                birdTypes.Add(bird.type);
            }

            bgL.SetBough(birdTypes.ToArray());
            //bgL.SetBough(birdTypes.ToArray());

        }

        Vector3 pos = boughStartPositionLeft;

        for (int i = 0; i < boughGrap.Count; i++)
        {
            boughGrap[i].transform.position = pos;

            pos.y += boughDistance.y;

            boughGrap[i].index = i;
        }

        //Debug.Log(pos);

        //CreateBoughRight(boughs);
    }

    public void CreateBoughRight(List<GameLogic.Bough> boughs)
    {
        foreach (GameLogic.Bough b in boughs)
        {
            Bough bgR = Instantiate(prefabBoughRight);

            boughGrap.Add(bgR);

            List<int> birdTypes = new List<int>();

            foreach (var bird in b.birds)
            {
                birdTypes.Add(bird.type);
            }

            bgR.SetBough(birdTypes.ToArray());

        }

        Vector3 posR = boughStartPositionRight;

        for (int i = 0; i < boughGrap.Count; i++)
        {
            boughGrap[i].transform.position = posR;

            posR.y += boughDistance.y;

            boughGrap[i].index = i;
        }

        Debug.Log(posR);
    }    

    public void RefreshBough(List<GameLogic.Bough> boughs)
    {
        for (int i = 0; i < boughs.Count; i++)
        {
            GameLogic.Bough gb = boughs[i];
            Bough boughGraphic = boughGrap[i];

            List<int> birdTypes = new List<int>();

            foreach (var bird in gb.birds)
            {
                birdTypes.Add(bird.type);
            }

            boughGraphic.SetBough(birdTypes.ToArray());
        }
    }

    public void OnClickBough(int boughIndex)
    {
        //Debug.Log("Click bough index: " + boughIndex);

        if (isSwichingBall)
            return;

        if (selectBoughIndex == -1)
        {
            if (_game.boughes[boughIndex].birds.Count != 0)
            {
                selectBoughIndex = boughIndex;
                StartCoroutine(MoveBirdUp(boughIndex));
            } 
        }
        else
        {
            if (selectBoughIndex == boughIndex)
            {
                StartCoroutine(MoveBirdDown(boughIndex));
                selectBoughIndex = -1;
            }
            else
            {
                StartCoroutine(SwithBirdCoroutine(selectBoughIndex, boughIndex));
            }
        }
    }

    private IEnumerator MoveBirdUp(int boughIndex)
    {
        isSwichingBall = true;
        Vector3 upPos = boughGrap[boughIndex].GetBoughPosition();

        List<GameLogic.Bird> birdList = _game.boughes[boughIndex].birds;

        GameLogic.Bird b = birdList[birdList.Count - 1];

        Vector3 birdPosition = boughGrap[boughIndex].GetBirdPosition(birdList.Count - 1);
        
        boughGrap[boughIndex].SetGraphicNone(birdList.Count - 1);

        previewBird.SetBird(b.type);

        previewBird.transform.position = birdPosition;

        previewBird.gameObject.SetActive(true);

        while(Vector3.Distance(previewBird.transform.position, upPos) > 0.005f)
        {
            previewBird.transform.position = Vector3.MoveTowards(previewBird.transform.position, upPos, 3 * Time.deltaTime);
            yield return null;
        }

        isSwichingBall = false;
    }

    private IEnumerator MoveBirdDown(int boughIndex)
    {
        isSwichingBall = true;

        List<GameLogic.Bird> birdList = _game.boughes[boughIndex].birds;

        Vector3 downPos = boughGrap[boughIndex].GetBirdPosition(birdList.Count - 1);

        Vector3 birdPositon = boughGrap[boughIndex].GetBoughPosition();

        previewBird.transform.position = birdPositon;

        while (Vector3.Distance(previewBird.transform.position, downPos) > 0.005f)
        {
            previewBird.transform.position = Vector3.MoveTowards(previewBird.transform.position, downPos, 3 * Time.deltaTime);
            yield return null;
        }

        previewBird.gameObject.SetActive(false);

        GameLogic.Bird b = birdList[birdList.Count - 1];

        boughGrap[boughIndex].SetGraphic(birdList.Count - 1, b.type);

        isSwichingBall = false;

    }

    private IEnumerator SwithBirdCoroutine(int fromBoughIndex, int toBoughIndex)
    {
        isSwichingBall = true;
       List<GameLogic.SwithBirdCommand> commands = _game.CheckSwithBird(fromBoughIndex, toBoughIndex);

        if(commands.Count == 0)
        {
            Debug.Log("Can't move");
        }
        else
        {
            pendingBirds = commands.Count;

            previewBird.gameObject.SetActive(false);

            for (int i = 0; i < commands.Count; i++)
            {
                GameLogic.SwithBirdCommand command = commands[i];
                Queue<Vector3> moveQueue = GetCommandPath(command);

                if( i == 0 )
                {
                    moveQueue.Dequeue();
                }
                StartCoroutine(SwithBird(command, moveQueue));
                yield return new WaitForSeconds(0.06f);
            }

            while(pendingBirds > 0)
            {
                yield return null;
            }

            //Debug.Log("Moving completed");

            _game.SwitchBird(fromBoughIndex, toBoughIndex);

        }

        selectBoughIndex = -1;
        isSwichingBall = false;
    }

    private int pendingBirds = 0;

    private Queue<Vector3> GetCommandPath(GameLogic.SwithBirdCommand command)
    {
        Queue<Vector3> queueMovement = new Queue<Vector3>();
        queueMovement.Enqueue(boughGrap[command.fromBoughIndex].GetBirdPosition(command.fromBirdIndex));
        queueMovement.Enqueue(boughGrap[command.fromBoughIndex].GetBoughPosition());
        queueMovement.Enqueue(boughGrap[command.toBoughIndex].GetBoughPosition());
        queueMovement.Enqueue(boughGrap[command.toBoughIndex].GetBirdPosition(command.toBirdIndex));

        return queueMovement;
    }

    private IEnumerator SwithBird(GameLogic.SwithBirdCommand command, Queue<Vector3> movement) 
    {
        boughGrap[command.fromBoughIndex].SetGraphicNone(command.fromBirdIndex);

        Vector3 spawnPos = movement.Peek();

        var birdObject = Instantiate(birdGraphic,spawnPos,Quaternion.identity);

        birdObject.SetBird(command.type);

        while(movement.Count > 0)
        {
            Vector3 target = movement.Dequeue();

            while (Vector3.Distance(birdObject.transform.position, target) > 0.005f)
            {
                birdObject.transform.position = Vector3.MoveTowards(birdObject.transform.position, target, 3f * Time.deltaTime);
                yield return null;
            }
        }

        yield return null;

        Destroy(birdObject.gameObject);

        boughGrap[command.toBoughIndex].SetGraphic(command.toBirdIndex, command.type);

        pendingBirds--;
    }
}

