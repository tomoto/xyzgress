using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Model;

public interface GameProvider {
    bool IsGameOver { get; }
    ItemInventoryModel PlayerInventory { get; }
    AchievementModel PlayerAchievement { get; }

    Camera MainCamera { get; }
    Scoreboard Scoreboard { get; }

    Burst FireBurster(Faction faction, Vector2 position, AchievementModel ownerAchievement);

    void CapturePortal(Portal portal, Faction newFaction);

    IEnumerable<Portal> FindLinkablePortals(Faction faction, KeyInventoryModel inventory, Portal source);
    void CreateLink(Faction faction, KeyInventoryModel inventory, Portal source, Portal target, AchievementModel achievement);
}
