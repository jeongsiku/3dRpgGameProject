using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorMode
{
    One = 1,
    Inverse = 2,
    NumberOfTimes = 4,
    Pingpong = 8
}

public class SpriteColor : MonoBehaviour
{
    private Material rgbMaterial;
    private ColorMode colorMode = ColorMode.One ;

    private List<Renderer> renderers = new List<Renderer>();

    private bool execute = false;
    private float speed = 1.0f;
    private float elapsed = 0;
    private Color start;
    private Color end;

    private bool keepAlpha = false;
    private int count = 1;

    public void Init()
    {
        renderers.AddRange( GetComponentsInChildren<Renderer>(true));

        Material mat = Resources.Load<Material>("Materials/RGBShader");

        rgbMaterial = Instantiate(mat);

        for(int i = 0; i < renderers.Count; i++)
        {
            if(renderers[i].tag != "Shadow")
                renderers[i].material = rgbMaterial;
        }
    }

    public void Execute(Color start, Color end, ColorMode colorMode, float speed,
                        bool keepAlpha = false, int count =1, bool inverse = false)
    {
        if (execute) return;
        this.start = start;
        this.end = end;
        this.colorMode = colorMode;
        this.speed = speed;
        this.keepAlpha = keepAlpha;
        this.count = count;
        if ((colorMode & ColorMode.Pingpong) == ColorMode.Pingpong)
            this.count *=2;
        execute = true;
        elapsed = 0;
        if(inverse)
        {
            Color temp = this.start;
            this.start = this.end;
            this.end = temp;
        }

    }

    void Update()
    {
        if (execute == false)
            return;

        elapsed += Time.deltaTime / speed;
        Color color = Color.Lerp(start, end, elapsed);
        rgbMaterial.SetFloat("_R", color.r);
        rgbMaterial.SetFloat("_G", color.g);
        rgbMaterial.SetFloat("_B", color.b);
        rgbMaterial.SetFloat("_A", color.a);

        if(elapsed >= 1)
        {
            elapsed = 0;
            if(colorMode != ColorMode.Pingpong)
            {
                if(keepAlpha != true)
                {
                    rgbMaterial.SetFloat("_R", 0);
                    rgbMaterial.SetFloat("_G", 0);
                    rgbMaterial.SetFloat("_B", 0);
                    rgbMaterial.SetFloat("_A", 1);
                }
            }
            
            if(colorMode == ColorMode.One)
            {
                execute = false;
            }
            else if((colorMode & ColorMode.Pingpong)== ColorMode.Pingpong)
            {
                Color temp = start;
                start = end;
                end = temp;

                if ((colorMode & ColorMode.NumberOfTimes)== ColorMode.NumberOfTimes)
                {
                    count--;
                    if(count <= 0)
                    {
                        execute=false;
                    }
                }
            }
            else if ((colorMode & ColorMode.NumberOfTimes) == ColorMode.NumberOfTimes)
            {
                count--;
                if (count <= 0)
                {
                    execute = false;
                }
            }

            //switch(colorMode)
            //{
            //    case ColorMode.One:

            //        break;
            //    case ColorMode.Pingpong:
            //        {

            //        }
            //        break;
            //}
        }
    }
}
