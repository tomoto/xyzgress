using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Model;

public abstract class AgentBase : MonoBehaviour
{
    protected GameProvider GameProvider { get; private set; }
    protected Burst CurrentBurst;
    protected abstract Faction SelfFaction { get; }
    protected abstract AchievementModel Achievement { get; }

    private TimeTicker rechargeTimer = new TimeTicker(GameConstants.RechargeInterval);

    protected AgentBase Init(GameProvider gameProvider)
    {
        GameProvider = gameProvider;
        return this;
    }

    // Use this for initialization
    protected virtual void Start()
    {
        GetComponent<Renderer>().material = FactionManager.GetInstance().GetSolidMaterial(SelfFaction);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (GameProvider.IsGameOver)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public virtual void SetPosition(Vector2 position)
    {
        transform.position = transform.position.Set2D(position);
    }

    protected bool IsBursterReady()
    {
        return CurrentBurst == null || !CurrentBurst.IsActive;
    }

    protected void FireBurster()
    {
        CurrentBurst = GameProvider.FireBurster(SelfFaction, transform.position, Achievement);
    }

    protected void TryCaptureOrRecharge(Portal portal, AchievementModel achievement)
    {
        if (portal.Model.Faction == Faction.None)
        {
            GameProvider.CapturePortal(portal, SelfFaction);

            achievement.Capture(portal.Model);
        }
        else if (portal.Model.Faction == SelfFaction)
        {
            if (portal.Energy < 100 && rechargeTimer.IsTimeout())
            {
                portal.RechargeEnergy(10);
                rechargeTimer.Start();

                achievement.Recharge();
            }
        }
    }
}
