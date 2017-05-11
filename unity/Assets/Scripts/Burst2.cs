using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst2 : Burst {
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
        var rr = Mathf.CeilToInt(maxRadius * Util.PixelsPerUnit);
        var textures = new Texture2D[rr];

        for (int r = 0; r < rr; r++)
        {
            var texture = Util.CreateTexture(r * 2, r * 2);
            Util.DrawPixelatedCircle(r, r, r, (x, y) => texture.SetPixel(x, y, c));
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

        var texture = BurstTextures[r];
        GetComponent<MeshRenderer>().material.mainTexture = texture;
        transform.localScale = new Vector2(texture.width, texture.height) / Util.PixelsPerUnit;
    }
}
