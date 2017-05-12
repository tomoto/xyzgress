using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public abstract class Burst : MonoBehaviour {
    public float MaxRadius = 3.0f;

    public float Velocity { get; private set; }
    public Faction Faction { get; private set; }
    public AchievementModel OwnerAchievement { get; private set; }
    public float Radius { get; protected set; }
    public bool IsActive { get; private set; }

    public Burst Init(Faction faction, Vector2 position, AchievementModel ownerAchievement, float velocity)
    {
        Faction = faction;
        OwnerAchievement = ownerAchievement;
        transform.position = transform.position.Set2D(position);
        Velocity = velocity;
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
