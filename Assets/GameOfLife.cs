using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;
    private Color[] Pixels;

    public Texture2D Life;


    int frame=0;

    //Parcourir toutes les cases autour de la case de coordonn�es x,y et faire deux variables o� sont stock�s les deux r�sultats finaux
    int[] Parcourir(int x, int y)
    {
        int[] voisins = new int[2];
        int nbrBlack = 0;
        int nbrWhite = 0;
        
        for (int index1 = -1;index1 <=1 ;index1++)
        {
            if (x+index1 < 0)
            {
                index1 = index1 + Width;
            }
            else if (x+index1 > Width)
            {
                index1 = index1 - Width;
            }
            else
            {
                for (int index2 = -1; index2 <= 1; index2++)            
                {
                    if (y + index2 < 0)
                    {
                        index2 = index2 + Height;
                    }
                    else if (y + index2 > Height)
                    {
                        index2 = index2 - Height;
                    }
                    else
                    {
                        var color = GetValue(Pixels, x + index1, y + index2);
                        //Color[(x+index1,y+index2)]
                        if (color == Color.black)
                        {
                            nbrBlack++;
                        }
                        else if (color == Color.white)
                        {
                            nbrWhite++;
                        }
                    }
                }
            }

        }
        voisins[0] = nbrBlack;
        voisins[1] = nbrWhite;

        return voisins;
    }
    //Color.white = rien 
    //Color.black = une cellule

    // Start is called before the first frame update
    void Start()
    {
        int seed = (int)Time.time;
        System.Random rnd = new System.Random(seed);
        

        Pixels = new Color[Width * Height];

        //Debug.Log("Size = " + Pixels.Length);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int rdmNbr = rnd.Next(0, 10);
                //Debug.Log("Random" + rdmNbr);

                if ( rdmNbr > 2)
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
        
        Color[] newGeneration = new Color[Width * Height];
        
        for (int y = 0; y < Height; y++)                                                 //ICI mettre y a 0 pour les deux
        {
            for (int x = 0; x < Width; x++)                                              //ICI mettre index1 a 0 pour les deux
            {
                var color = GetValue(Pixels, x , y );

                SetValue(ref newGeneration, x, y, color);

                if (color == Color.black)
                {
                    var voisins = Parcourir(x, y);
                    int nbrBlack = voisins[0];
                    int nbrWhite = voisins[1];
                    nbrBlack = nbrBlack - 1;
                    if (nbrBlack < 2 || nbrBlack > 3 ) 
                    {
                        SetValue(ref newGeneration, x, y, Color.white);
                        //Debug.Log($"Pixel {x},{y}: DEAD!");
                    }
                    

                }
                else if (color == Color.white)
                {
                    var voisins = Parcourir(x, y);
                    int nbrBlack = voisins[0];
                    int nbrWhite = voisins[1];
                    nbrWhite = nbrWhite - 1;
                    if (nbrBlack == 3)
                    {
                        SetValue(ref newGeneration, x, y, Color.black);
                        //Debug.Log($"Pixel {x},{y}: ALIVE!");
                    }
                }
            }
        }

        
        Debug.Log("Frame " + frame++);

        //Thread.Sleep(500);

        Pixels = newGeneration;
        SetValue(ref Pixels, 0, 0, Color.red);
        Life.SetPixels(Pixels);
        Life.Apply();
        SetSpriteTexture();
        
    }
}