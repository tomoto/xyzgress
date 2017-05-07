using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Burst : MonoBehaviour {
    public float MaxRadius = 3.0f;
    public float Velocity = 3.0f;

    public Faction Faction { get; private set; }
    public AchievementModel OwnerAchievement { get; private set; }
    public float Radius { get; private set; }
    public bool IsActive { get; private set; }

    public Burst Init(Faction faction, Vector2 position, AchievementModel ownerAchievement)
    {
        Faction = faction;
        OwnerAchievement = ownerAchievement;
        transform.position = position;
        return this;
    }

	// Use this for initialization
	void Start() {
        Radius = 0;
        IsActive = true;
        // GetComponent<Renderer>().material = FactionManager.GetInstance().GetSolidMaterial(Faction);
    }

    // Update is called once per frame
    void Update()
    {
        Radius += Velocity * Time.deltaTime;

        if (Radius < MaxRadius)
        {
            DrawLine(Radius);
            GetComponent<CircleCollider2D>().radius = Radius;
        }
        else
        {
            Destroy(gameObject);
            IsActive = false;
        }
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
