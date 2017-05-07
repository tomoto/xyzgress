using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Model;

public class Player : AgentBase
{
    public float Speed = 4;

    private IList<Portal> CollidedPortals = new List<Portal>();
    private Portal CurrentPortal;

    protected override Faction SelfFaction { get { return Faction.Player; } }
    protected override AchievementModel Achievement { get { return GameProvider.PlayerAchievement; } }
    private ItemInventoryModel Inventory { get { return GameProvider.PlayerInventory; } }
    private bool IsHaijin { get { return InitialMenuController.IsHaijin; } }

    private int prevDX;
    private int prevDY;

    public new Player Init(GameProvider gameProvider)
    {
        base.Init(gameProvider);
        return this;
    }

    public override void SetPosition(Vector2 position)
    {
        base.SetPosition(position);
        CenterPlayer();
    }

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();

        if (GameProvider.IsGameOver) return;

        var dx = InputUtil.GetX();
        var dy = InputUtil.GetY();

        Move(dx, dy);
        CenterPlayer();

        if (Input.GetButton(InputUtil.Button1))
        {
            TryAttack();
        }
        else
        {
            if (IsHaijin)
            {
                CurrentBurst = null;
            }
        }

        if (Input.GetButtonDown(InputUtil.Button2))
        {
            TryCreateLink();
        }

        if (CurrentPortal != null)
        {
            TryCaptureOrRecharge(CurrentPortal, Achievement);
        }
    }

    private void CenterPlayer()
    {
        GameProvider.MainCamera.transform.position =
            GameProvider.MainCamera.transform.position.Set2D(transform.position);
    }

    private void Move(int dx, int dy)
    {
        if (dx != 0 || dy != 0)
        {
            if (dx != 0 && dy != 0)
            {
                if (prevDX != dx)
                {
                    dy = 0;
                }
                else
                {
                    dx = 0;
                }
            }
            else
            {
                prevDX = dx;
                prevDY = dy;
            }

            transform.rotation = Util.Get4WayRotation(dx, dy);
        }

        var rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(dx, dy) * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var portal = collision.GetComponent<Portal>();
        if (portal != null)
        {
            TryCaptureOrRecharge(portal, Achievement);
            TryHack(portal);

            CollidedPortals.Add(portal);
            UpdateCurrentPortal();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var portal = collision.GetComponent<Portal>();
        if (portal != null)
        {
            CollidedPortals.Remove(portal);
            UpdateCurrentPortal();
        }
    }

    private void UpdateCurrentPortal()
    {
        CurrentPortal = GetClosestPortalTo(CollidedPortals, transform.position);
    }

    private Portal GetClosestPortalTo(IList<Portal> portals, Vector2 position)
    {
        if (portals.Any())
        {
            return portals.Aggregate((p1, p2) => (position - p1.Model.Position).sqrMagnitude < (position - p2.Model.Position).sqrMagnitude ? p1 : p2);
        }
        else
        {
            return null;
        }
    }

    private void TryHack(Portal portal)
    {
        if (portal.TryHack(Faction.Player, Inventory))
        {
            portal.SetHasKey(Inventory.HasKey(portal.Model));
            GameProvider.Scoreboard.DisplayBursters(Inventory.Bursters);
            GameProvider.Scoreboard.DisplayHack();
            Achievement.Hack(portal.Model);
        }
    }

    private void TryCreateLink()
    {
        if (CurrentPortal == null)
        {
            return;
        }

        var targetPortal = GameProvider.FindLinkablePortals(Faction.Player, Inventory, CurrentPortal).FirstOrDefault();

        if (targetPortal != null)
        {
            GameProvider.CreateLink(Faction.Player, Inventory, CurrentPortal, targetPortal, Achievement);
            Inventory.RemoveKey(targetPortal.Model);
            targetPortal.SetHasKey(false);
        }
    }

    private void TryAttack()
    {
        if (Inventory.Bursters > 0 && IsBursterReady())
        {
            Inventory.AddBursters(-1);
            GameProvider.Scoreboard.DisplayBursters(Inventory.Bursters);
            FireBurster();
        }
    }
}
