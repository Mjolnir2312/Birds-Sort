using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBird(int type)
    {
        if (type == 0)
        {
            spriteRenderer.color = new Color(0,0, 0, 0);
        }
        else
        {
            int index = type -1;

            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = sprites[index];
        }
    }
}
