using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Burst1 : Burst {

	// Use this for initialization
	protected override void Start() {
        base.Start();
        // GetComponent<Renderer>().material = FactionManager.GetInstance().GetSolidMaterial(Faction);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!IsActive) return;

        DrawLine(Radius);
        GetComponent<CircleCollider2D>().radius = Radius;
    }

    private void DrawLine(float radius)
    {
        var lineRenderer = GetComponent<LineRenderer>();

        int segments = Mathf.CeilToInt(radius * 8 + 8);
        var points = new Vector3[segments + 1];
        for (int i = 0; i < segments; i++)
        {
            var r = Mathf.PI * 2 / segments * i;
            points[i] = transform.position + new Vector3(Mathf.Cos(r) * radius, Mathf.Sin(r) * radius);
        }
        points[segments] = points[0];

        lineRenderer.numPositions = points.Length;
        lineRenderer.SetPositions(points);
    }
}
