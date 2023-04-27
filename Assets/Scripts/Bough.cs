using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bough : MonoBehaviour
{
    [SerializeField] private GameGraphic gameGraphic;
    [SerializeField] private Bird[] birds;
    [SerializeField] private Transform boughTransform;

    public int index;

    private void OnMouseUpAsButton()
    {
        gameGraphic.OnClickBough(index);
    }

    public void SetBough(int[] birdTypes)
    {
       for (int i = 0; i < birds.Length; i++)
        {
            if (i >= birdTypes.Length)
            {
                SetGraphicNone(i);
            }
            else
            {
                SetGraphic(i, birdTypes[i]);
            }
        }
    }

    public void SetGraphic(int index, int type)
    {
        birds[index].SetBird(type);
    }

    public void SetGraphicNone(int index)
    {
        birds[index].SetBird(0);
    }

    public Vector3 GetBirdPosition(int index)
    {
        return birds[index].transform.position;
    }

    public Vector3 GetBoughPosition()
    {
        return boughTransform.position;
    }
}
