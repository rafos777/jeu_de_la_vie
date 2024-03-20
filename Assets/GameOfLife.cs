using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;

    private Color[] Pixels;

    public Texture2D Life;

    //Color.white = rien 
    //Color.black = une cellule

    // Start is called before the first frame update
    void Start()
    {
        Pixels = new Color[Width * Height];

        //Debug.Log("Size = " + Pixels.Length);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if((x+y)%2==0)
                {
                    SetValue(ref Pixels, x, y, Color.black);
                }
                else
                {
                    SetValue(ref Pixels, x, y, Color.white);
                }
            }
        }

        Life = new Texture2D(Width, Height);
        Life.filterMode = FilterMode.Point;

    }

    void SetSpriteTexture()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite newSprite = Sprite.Create(Life, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite = newSprite;
    }

    Color GetValue(Color[] iPixels, int X, int Y)
    {
        return iPixels[X + Y * Width];
    }

    void SetValue(ref Color[] iPixels, int X, int Y, Color iColor)
    {
        iPixels[X + Y * Width] = iColor;
    }

    // Update is called once per frame
    void Update()
    {
        // XXX

        Life.SetPixels(Pixels);
        Life.Apply();
        SetSpriteTexture();
    }
}