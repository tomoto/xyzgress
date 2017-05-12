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

    private KeyInventoryModel inventory = new KeyInventoryModel();
    private Portal attackTargetPortal;

    protected override AchievementModel Achievement { get { return AchievementModel.DummyAchievement; } }

    public new NPCAgentBase Init(GameProvider gameProvider)
    {
        base.Init(gameProvider);
        return this;
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
        var r = Random.value;
        Direction = new Vector2(r < 0.25 ? -1 : r < 0.5 ? 1 : 0, r < 0.5 ? 0 : r < 0.75 ? -1 : 1);
    }
}
