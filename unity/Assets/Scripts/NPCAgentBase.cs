using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Model;

public abstract class NPCAgentBase : AgentBase
{
    public float Speed = 3;
    public Vector2 Direction = Vector3.up;
    public float KeyDropRate = 0.8f;
    public float CreateLinkRate = 0.4f;
    public float ChangeDirectionRate = 0.02f;
    public float PortalAttackRate = 0.25f;
    public float PortalAttackMaxDistance = 3.0f;

    public float territoryMinX = -999;
    public float territoryMaxX = 999;

    private KeyInventoryModel inventory = new KeyInventoryModel();
    private Portal attackTargetPortal;

    protected override AchievementModel Achievement { get { return AchievementModel.DummyAchievement; } }

    public new NPCAgentBase Init(GameProvider gameProvider)
    {
        base.Init(gameProvider);
        return this;
    }

    public void SetTerritory(float minX, float maxX)
    {
        territoryMinX = minX;
        territoryMaxX = maxX;
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();

        if (GameProvider.IsGameOver) return;

        if (attackTargetPortal != null)
        {
            if ((attackTargetPortal.Model.Position - transform.position.To2D()).magnitude > PortalAttackMaxDistance ||
                attackTargetPortal.Model.Faction == Faction.None || attackTargetPortal.Model.Faction == SelfFaction)
            {
                // End attack
                attackTargetPortal = null;
            }
            else
            {
                TryAttack();
            }

            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
        else
        {
            if (Random.value < ChangeDirectionRate)
            {
                ChangeDirection();
            }

            transform.rotation = Util.Get4WayRotation(Direction.x, Direction.y);

            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = Direction * Speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ChangeDirection();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ChangeDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var portal = collision.GetComponent<Portal>();
        if (portal != null)
        {
            TryCaptureOrRecharge(portal, AchievementModel.DummyAchievement);

            if (Random.value < KeyDropRate)
            {
                inventory.AddKey(portal.Model);
            }

            if (portal.Model.Faction == SelfFaction)
            {
                if (Random.value < CreateLinkRate)
                {
                    TryCreateLink(portal);
                }
            }
            else if (portal.Model.Faction != Faction.None)
            {
                if (Random.value < PortalAttackRate * (1 + portal.Model.NumberOfLinks))
                {
                    TryCommenceAttack(portal);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var portal = collision.GetComponent<Portal>();
        if (portal != null)
        {
            TryCaptureOrRecharge(portal, AchievementModel.DummyAchievement);
        }
    }

    private void TryCreateLink(Portal portal)
    {
        var targetPortal = GameProvider.FindLinkablePortals(SelfFaction, inventory, portal).FirstOrDefault();
        if (targetPortal != null)
        {
            GameProvider.CreateLink(SelfFaction, inventory, portal, targetPortal, AchievementModel.DummyAchievement);
            inventory.RemoveKey(targetPortal.Model);
        }
    }

    private void TryCommenceAttack(Portal portal)
    {
        if (attackTargetPortal == null)
        {
            attackTargetPortal = portal;
        }
    }

    private void TryAttack()
    {
        if (IsBursterReady())
        {
            FireBurster(GameConstants.NPCBursterVelocity);
        }
    }

    private void ChangeDirection()
    {
        const float margin = 3;
        var x = transform.position.x;

        System.Func<float,float> f = (center) => Util.Sigmoid((x - center));

        var xt = 0.25 * (f(territoryMinX - margin) + f(territoryMaxX + margin)); // between 0 and 0.5

        // Debug.Log(string.Format("Territory=({0},{1}), x={2}, xt={3}", territoryMinX, territoryMaxX, x, xt));

        var r = Random.value;
        var dx = r < xt ? -1 : r < 0.5 ? 1 : 0;
        var dy = r < 0.5 ? 0 : r < 0.75 ? -1 : 1;

        Direction = new Vector2(dx, dy);
    }
}
