using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Model;

public class GameController : MonoBehaviour, GameProvider
{
    public GameObject BlockPrefab;
    public Portal PortalPrefab;
    public Link LinkPrefab;
    public CF CFPrefab;
    public FloatingText FloatingMUPrefab;
    public Burst BurstPrefab;
    public Player PlayerPrefab;
    public EnemyAgent EnemyAgentPrefab;
    public FriendlyAgent FriendlyAgentPrefab;

    public GameObject FieldLayer;
    public GameObject AgentLayer;
    public GameObject LinkLayer;
    public Camera MainCamera { get; private set; }
    public Scoreboard Scoreboard { get; private set; }
    public Resultboard Resultboard { get; private set; }

    public readonly FieldModel Field = FieldFactory.CreateDefaultField(InitialMenuController.Level);
    public readonly LinkManager LinkManager = new LinkManager();
    public readonly IList<Portal> Portals = new List<Portal>();
    public readonly IList<Link> Links = new List<Link>();
    public readonly IList<CF> CFs = new List<CF>();

    public ItemInventoryModel PlayerInventory { get { return playerInventory; } }
    public AchievementModel PlayerAchievement { get { return playerAchievement; } }

    private float startTime;
    private int days;
    public bool IsGameOver { get; private set; }

    private TimeTicker gameOverScreenWaitTimer = new TimeTicker(GameConstants.GameOverScreenWaitTime);
    private ItemInventoryModel playerInventory = new ItemInventoryModel();
    private AchievementModel playerAchievement = new AchievementModel();

    // Use this for initialization
    void Start() {
        InputUtil.Clear();

        MainCamera = GameObject.Find(GameConstants.MainCamera).GetComponent<Camera>();
        Scoreboard = transform.Find("Scoreboard").GetComponent<Scoreboard>();
        Resultboard = transform.Find("Resultboard").GetComponent<Resultboard>();

        PopulateField();
        PlayerInventory.Bursters = GameConstants.InitialPlayerBursters;
        Scoreboard.DisplayBursters(PlayerInventory.Bursters);

        startTime = Time.time;
        IsGameOver = false;
    }

    // Update is called once per frame
    void Update() {
        if (IsGameOver)
        {
            WaitForButtonAndGoBackToMenu();
            return; // nothing to do
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FinishGame(true);
        }

        DestroyDecayedPortals();

        int newDays = Mathf.FloorToInt((Time.time - startTime) / GameConstants.SecondsPerDay);
        if (newDays > days)
        {
            days = newDays;

            Scoreboard.DisplayDays(days);

            if (days >= GameConstants.DaysPerGame)
            {
                FinishGame(false);
            }
            else
            {
                SoundManager.GetInstance().DayUpSound.Play();
            }
        }

        if (PlayerAchievement.IsAPChanged)
        {
            Scoreboard.DisplayLevelAndAP(PlayerAchievement.Level, PlayerAchievement.AP);

            if (PlayerAchievement.IsLevelChanged)
            {
                SoundManager.GetInstance().LevelUpSound.Play();
            }

            PlayerAchievement.ResetAPChanged();
        }
    }

    private void WaitForButtonAndGoBackToMenu()
    {
        if (gameOverScreenWaitTimer.IsTimeout() &&
                (InputUtil.GetButtonDown(0) || InputUtil.GetButtonDown(1)))
        {
            SceneManager.LoadScene(GameConstants.MenuScene);
        }
    }

    private void DestroyDecayedPortals()
    {
        foreach (var portal in Portals)
        {
            if (portal.Model.Faction != Faction.None && portal.Energy == 0)
            {
                DestroyPortal(portal);
            }
        }
    }

    private void PopulateField()
    {
        for (int y = 0; y < Field.Height; y++)
        {
            for (int x = 0; x < Field.Width; x++)
            {
                switch (Field.GetItemAt(x, y))
                {
                    case FieldModel.FieldItem.Block:
                        var block = Util.Instantiate(BlockPrefab, FieldLayer);
                        block.transform.position = new Vector2(x, y);
                        break;
                    case FieldModel.FieldItem.Portal:
                        var portalModel = LinkManager.AddPortal(new Vector2(x, y), Faction.Enemy);
                        Portals.Add(Util.Instantiate(PortalPrefab, FieldLayer).Init(portalModel));
                        break;
                    case FieldModel.FieldItem.Player:
                        var player = Util.Instantiate(PlayerPrefab, AgentLayer).Init(this);
                        player.SetPosition(new Vector2(x, y));
                        break;
                    case FieldModel.FieldItem.Friendly:
                        var friendly = Util.Instantiate(FriendlyAgentPrefab, AgentLayer).Init(this);
                        friendly.SetPosition(new Vector2(x, y));
                        break;
                    case FieldModel.FieldItem.Enemy:
                        var enemy = Util.Instantiate(EnemyAgentPrefab, AgentLayer).Init(this);
                        enemy.SetPosition(new Vector2(x, y));
                        break;
                }
            }
        }
    }

    public IEnumerable<Portal> FindLinkablePortals(Faction faction, KeyInventoryModel inventory, Portal source)
    {
        var linkablePortalModels = LinkManager.FindLinkablePortals(faction, inventory, source.Model);
        return linkablePortalModels.Select(ModelToView);
    }

    public void CreateLink(Faction faction, KeyInventoryModel inventory, Portal source, Portal target, AchievementModel achievement)
    {
        // Link
        var linkModel = LinkManager.AddLink(faction, inventory, source.Model, target.Model);
        Debug.Log(string.Format("Link {0} created.", linkModel));

        Links.Add(Util.Instantiate(LinkPrefab, LinkLayer).Init(linkModel));
        achievement.CreateLink();

        // CF
        var cfModels = LinkManager.AddCFs(linkModel);

        if (cfModels.Any())
        {
            var mu = cfModels.Sum(cfModel => cfModel.MU);

            Util.Instantiate(FloatingMUPrefab, null).Init(source.Model.Position, Mathf.FloorToInt(mu) + " MU", 0.8f);

            Debug.Log(string.Format("{0} CFs created total {1} MU", cfModels.Count(), mu));
            Scoreboard.DisplayMU(faction, LinkManager.GetMU(faction));

            foreach (var cfModel in cfModels)
            {
                CFs.Add(Util.Instantiate(CFPrefab, LinkLayer).Init(cfModel));
                achievement.CreateCF(Mathf.FloorToInt(cfModel.MU));
            }

            SoundManager.GetInstance().CFCreatedSound.Play();
        }
    }

    private Portal ModelToView(PortalModel model)
    {
        return Portals.First(vm => vm.Model == model);
    }

    private Link ModelToView(LinkModel model)
    {
        return Links.First(vm => vm.Model == model);
    }

    private CF ModelToView(CFModel model)
    {
        return CFs.First(vm => vm.Model == model);
    }

    public void DestroyPortal(Portal portal, Faction newFaction = Faction.None)
    {
        Faction destroyedFaction = portal.Model.Faction;

        // First, process the model for links and CFs
        IList<LinkModel> destroyedLinkModels;
        IList<CFModel> destroyedCFModels;
        LinkManager.DestroyPortal(portal.Model, out destroyedLinkModels, out destroyedCFModels);
        var destroyedCFs = destroyedCFModels.Select(ModelToView).ToList();
        var destroyedLinks = destroyedLinkModels.Select(ModelToView).ToList();

        Debug.Log(string.Format("{0} links and {1} CFs ({2} MUs) are destroyed.", destroyedLinkModels.Count(), destroyedCFModels.Count(), destroyedCFModels.Sum(cf => cf.MU)));

        var achievement = portal.DestroyerAchievement ?? AchievementModel.DummyAchievement;
        achievement.DestroyPortal();

        // Second, process the view for links and CFs

        // CFs
        foreach (var cfs in destroyedCFs)
        {
            CFs.Remove(cfs);
            Destroy(cfs.gameObject);
            achievement.DestroyCF();
        }

        if (destroyedCFs.Any())
        {
            SoundManager.GetInstance().CFDestroyedSound.Play();
        }

        // Links
        foreach (var link in destroyedLinks)
        {
            Links.Remove(link);
            Destroy(link.gameObject);
            achievement.DestroyLink();
        }

        Scoreboard.DisplayMU(destroyedFaction, LinkManager.GetMU(destroyedFaction));

        // Finally, process the portal
        CapturePortal(portal, newFaction);

    }

    public void CapturePortal(Portal portal, Faction newFaction)
    {
        portal.Capture(newFaction);
    }

    public Burst FireBurster(Faction faction, Vector2 position, AchievementModel ownerAchievement, float velocity)
    {
        return Util.Instantiate(BurstPrefab, AgentLayer).Init(faction, position, ownerAchievement, velocity);
    }

    private void FinishGame(Boolean escape)
    {
        var muDiff = LinkManager.GetMU(Faction.Player) - LinkManager.GetMU(Faction.Enemy);
        var finalResult =
            (muDiff < 0 || escape) ? Resultboard.FinalResult.Lose :
            (muDiff >= GameConstants.ExceedingMUToWin) ? Resultboard.FinalResult.Win :
            Resultboard.FinalResult.Draw;

        Resultboard.DisplayFinalResult(finalResult, new MedalHelper(PlayerAchievement).Results);
        Resultboard.Show();
        SoundManager.GetInstance().FinishSound.Play();

        IsGameOver = true;
        if (!escape)
        {
            gameOverScreenWaitTimer.Start();
        }
    }
}
