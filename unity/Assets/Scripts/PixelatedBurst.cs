using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelatedBurst : Burst {
    private static Texture2D[] BurstTextures;

    private static void InitTextures(Color c, float maxRadius)
    {
        if (BurstTextures == null)
        {
            BurstTextures = CreateBurstTextures(c, maxRadius);
        }
    }
    
    private static Texture2D[] CreateBurstTextures(Color c, float maxRadius)
    {
        var rr = Mathf.CeilToInt(maxRadius * 8);
        var textures = new Texture2D[rr];

        for (int r = 0; r < rr; r++)
        {
            var texture = new Texture2D(r * 2 + 1, r * 2 + 1, TextureFormat.ARGB32, false);

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    texture.SetPixel(x, y, Color.clear);
                }
            }

            var a = r;
            for (var x = 0; x < r; x++)
            {
                var b = Mathf.FloorToInt(Mathf.Sqrt((r * r) - (x + 1) * (x + 1)));
                for (var p = b; p < Mathf.Max(a, b + 1); p++)
                {
                    texture.SetPixel(r + x, r + p, c);
                    texture.SetPixel(r - x, r + p, c);
                    texture.SetPixel(r + x, r - p, c);
                    texture.SetPixel(r - x, r - p, c);
                }
                a = b;
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            textures[r] = texture;
        }

        return textures;
    }

    public Color Color;

    // Use this for initialization
    protected override void Start () {
        base.Start();
        InitTextures(Color, MaxRadius);
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();

        if (!IsActive) return;

        var r = Mathf.FloorToInt(Radius * 8);

        GetComponent<MeshRenderer>().material.renderQueue = 3001;
        GetComponent<MeshRenderer>().material.mainTexture = BurstTextures[r];

        transform.localScale = new Vector2(r * 2 + 1, r * 2 + 1) / 8.0f;
    }
}
