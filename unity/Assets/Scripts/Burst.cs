using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public class Burst : MonoBehaviour {
    public float MaxRadius = 3.0f;
    public float Velocity = 3.0f;

    public Faction Faction { get; private set; }
    public AchievementModel OwnerAchievement { get; private set; }
    public float Radius { get; protected set; }
    public bool IsActive { get; private set; }

    public Burst Init(Faction faction, Vector2 position, AchievementModel ownerAchievement)
    {
        Faction = faction;
        OwnerAchievement = ownerAchievement;
        transform.position = position;
        return this;
    }

	// Use this for initialization
	protected virtual void Start() {
        Radius = 0;
        IsActive = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Radius += Velocity * Time.deltaTime;

        if (Radius >= MaxRadius)
        {
            Destroy(gameObject);
            IsActive = false;
        }
    }
}
