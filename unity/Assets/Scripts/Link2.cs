using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Link2 : Link
{
    public Color Color;

    // Use this for initialization
    void Start() {
        var p1 = Model.Source.Position * Util.PixelsPerUnit;
        var p2 = Model.Target.Position * Util.PixelsPerUnit;
        Rect b = Util.GetBoundingRect(p1, p2);

        // add margin to the bounding box
        b.position -= new Vector2(1, 1);
        b.size += new Vector2(3, 3);

        var texture = Util.CreateTexture((int)b.width, (int)b.height);
        var color = FactionManager.GetInstance().GetSolidMaterial(Model.Faction).color;
        Util.DrawPixelatedLine(p1 - b.position, p2 - b.position, (x, y) => texture.SetPixel(x, y, color));
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        GetComponent<MeshRenderer>().material.mainTexture = texture;
        transform.position = transform.position.Set2D(b.center / Util.PixelsPerUnit);
        transform.localScale = transform.localScale.Set2D(b.size / Util.PixelsPerUnit);
    }

    // Update is called once per frame
    void Update() {
        // Nothing to do
	}
}
