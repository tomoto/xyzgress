using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Link2 : Link
{
    public Color Color;

    // Use this for initialization
    void Start() {
        int x1 = (int)(Model.Source.Position.x * Util.PixelsPerUnit);
        int y1 = (int)(Model.Source.Position.y * Util.PixelsPerUnit);
        int x2 = (int)(Model.Target.Position.x * Util.PixelsPerUnit);
        int y2 = (int)(Model.Target.Position.y * Util.PixelsPerUnit);
        int x0 = Mathf.Min(x1, x2) - 1;
        int y0 = Mathf.Min(y1, y2) - 1;
        int w = Mathf.Abs(x2 - x1) + 3;
        int h = Mathf.Abs(y2 - y1) + 3;
        var texture = Util.CreateTexture(w, h);
        var color = FactionManager.GetInstance().GetSolidMaterial(Model.Faction).color;
        Util.DrawPixelatedLine(x1 - x0, y1 - y0, x2 - x0, y2 - y0, (x, y) => texture.SetPixel(x, y, color));
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        GetComponent<MeshRenderer>().material.mainTexture = texture;
        transform.position = new Vector3((x0 + w / 2.0f) / Util.PixelsPerUnit, (y0 + h / 2.0f) / Util.PixelsPerUnit, transform.position.z);
        transform.localScale = new Vector3(w, h, 1) / Util.PixelsPerUnit;
    }

    // Update is called once per frame
    void Update() {
        // TODO
	}
}
